using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace TDPG.GeoCoordConversion
{
    public class GridReference
    {
        public static readonly Regex OSGridLettersRegex = new Regex(@"^[HNOST][A-HJ-Z]$");
        public static readonly Regex OSGridNumbersRegex = new Regex(@"^\d{1,5}$");

        public long Northing { get; set; }
        public long Easting { get; set; }

        public GridReference(long easting, long northing)
        {
            Northing = northing;
            Easting = easting;
        }

        public GridReference(string osGridLetters, string osGridEasting, string osGridNorthing)
        {
            Easting = 0;
            Northing = 0;

            #region Validate Grid Reference
            if (!OSGridLettersRegex.IsMatch(osGridLetters))
            {
                throw new ApplicationException("First part of Grid Reference must be two letters.  First letter N, O, S or T.  Second letter A-Z, but not I.");
            }

            if (!OSGridNumbersRegex.IsMatch(osGridEasting))
            {
                throw new ApplicationException("Second part of Grid Reference must be one or more digits.");
            }

            if (!OSGridNumbersRegex.IsMatch(osGridNorthing))
            {
                throw new ApplicationException("Third part of Grid Reference must be one or more digits.");
            }
            #endregion

            #region Width
            switch (osGridLetters[0])
            {
                case 'S':
                case 'N':
                case 'H':
                    Easting += 0;
                    break;
                case 'T':
                case 'O':
                    Easting += 500000;
                    break;
            }

            switch (osGridLetters[1])
            {
                case 'A':
                case 'F':
                case 'L':
                case 'Q':
                case 'V':
                    Easting += 0;
                    break;
                case 'B':
                case 'G':
                case 'M':
                case 'R':
                case 'W':
                    Easting += 100000;
                    break;
                case 'C':
                case 'H':
                case 'N':
                case 'S':
                case 'X':
                    Easting += 200000;
                    break;
                case 'D':
                case 'J':
                case 'O':
                case 'T':
                case 'Y':
                    Easting += 300000;
                    break;
                case 'E':
                case 'K':
                case 'P':
                case 'U':
                case 'Z':
                    Easting += 400000;
                    break;
            }

            int gridDigit = Int32.Parse(osGridEasting);
            int depth = (int)Math.Pow(10, (5 - osGridEasting.Length));
            Easting += (gridDigit * depth);
            #endregion

            #region Height
            switch (osGridLetters[0])
            {
                case 'S':
                case 'T':
                    Northing += 0;
                    break;
                case 'N':
                case 'O':
                    Northing += 500000;
                    break;
                case 'H':
                    Northing += 1000000;
                    break;
            }

            switch (osGridLetters[1])
            {
                case 'V':
                case 'W':
                case 'X':
                case 'Y':
                case 'Z':
                    Northing += 0;
                    break;
                case 'Q':
                case 'R':
                case 'S':
                case 'T':
                case 'U':
                    Northing += 100000;
                    break;
                case 'L':
                case 'M':
                case 'N':
                case 'O':
                case 'P':
                    Northing += 200000;
                    break;
                case 'F':
                case 'G':
                case 'H':
                case 'J':
                case 'K':
                    Northing += 300000;
                    break;
                case 'A':
                case 'B':
                case 'C':
                case 'D':
                case 'E':
                    Northing += 400000;
                    break;
            }

            gridDigit = Int32.Parse(osGridNorthing);
            depth = (int)Math.Pow(10, (5 - osGridNorthing.Length));
            Northing += (gridDigit * depth);
            #endregion
        }

        public static PolarGeoCoordinate ChangeToPolarGeo(GridReference original)
        {
            return Converter.GridReferenceToGeodesic(original);
        }

        public bool IsTheSameAs(GridReference compareTo)
        {
            if (
                compareTo.Northing != this.Northing
                || compareTo.Easting != this.Easting
                )
                return false;
            else
                return true;
        }

        //overload of IsTheSameAs to ignore rounding errors on final digit
        public bool IsTheSameAs(GridReference compareTo, bool ignoreFinalDigit)
        {
            if (ignoreFinalDigit)
            {
                AlignSigFigs(compareTo);
                GridReference compareToReduced = ReduceSigFigsBy1(compareTo);
                GridReference comparer = ReduceSigFigsBy1(this);

                return comparer.IsTheSameAs(compareToReduced);
            }

            return IsTheSameAs(compareTo);
        }

        internal void AlignSigFigs(GridReference source)
        {
            int SigFigsSrc;
            int SigFigsDest;


            SigFigsSrc = Converter.GetSigFigs(source.Easting);
            SigFigsDest = Converter.GetSigFigs(this.Easting);

            this.Easting = (int)Converter.SetSigFigs(
                 (double)this.Easting,
                 SigFigsDest < SigFigsSrc ? SigFigsDest : SigFigsSrc);

            source.Easting = (int)Converter.SetSigFigs(
                  (double)source.Easting,
                 SigFigsDest < SigFigsSrc ? SigFigsDest : SigFigsSrc);

            SigFigsSrc = Converter.GetSigFigs(source.Northing);
            SigFigsDest = Converter.GetSigFigs(this.Northing);

            this.Northing = (int)Converter.SetSigFigs(
                  (double)this.Northing,
                 SigFigsDest < SigFigsSrc ? SigFigsDest : SigFigsSrc);

            source.Northing = (int)Converter.SetSigFigs(
                  (double)source.Northing,
                 SigFigsDest < SigFigsSrc ? SigFigsDest : SigFigsSrc);
        }

        internal static GridReference ReduceSigFigsBy1(GridReference original)
        {
            double Northing = Converter.SetSigFigs(
                 original.Northing,
                 Converter.GetSigFigs(original.Northing) - 1);

            double Easting = Converter.SetSigFigs(
                 original.Easting,
                 Converter.GetSigFigs(original.Easting) - 1);

            return new GridReference((int)Easting, (int)Northing);
        }

        public override string ToString()
        {
            string letters;
            string eastNumber;
            string northNumber;

            GetGridReference(out letters, out eastNumber, out northNumber);

            return String.Format("{0} {1} {2}", letters, eastNumber, northNumber);
        }

        public void GetGridReference(out string letters, out string eastNumber, out string northNumber)
        {
            long _easting = Easting;
            long _northing = Northing;

            #region
            if (_easting >= 500000)
            {
                _easting -= 500000;
                if (_northing >= 1000000)
                {
                    _northing -= 1000000;
                    letters = "J";
                }
                else if (_northing >= 500000)
                {
                    _northing -= 500000;
                    letters = "O";
                }
                else
                {
                    letters = "T";
                }
            }
            else
            {
                if (_northing >= 1000000)
                {
                    _northing -= 1000000;
                    letters = "H";
                }
                else if (_northing >= 500000)
                {
                    _northing -= 500000;
                    letters = "N";
                }
                else
                {
                    letters = "S";
                }
            }

            if (_easting >= 400000)
            {
                _easting -= 400000;
                if (_northing >= 400000)
                {
                    _northing -= 400000;
                    letters += "E";
                }
                else if (_northing >= 300000)
                {
                    _northing -= 300000;
                    letters += "K";
                }
                else if (_northing >= 200000)
                {
                    _northing -= 200000;
                    letters += "P";
                }
                else if (_northing >= 100000)
                {
                    _northing -= 100000;
                    letters += "U";
                }
                else
                {
                    letters += "Z";
                }
            }
            else if (_easting >= 300000)
            {
                _easting -= 300000;
                if (_northing >= 400000)
                {
                    _northing -= 400000;
                    letters += "D";
                }
                else if (_northing >= 300000)
                {
                    _northing -= 300000;
                    letters += "J";
                }
                else if (_northing >= 200000)
                {
                    _northing -= 200000;
                    letters += "O";
                }
                else if (_northing >= 100000)
                {
                    _northing -= 100000;
                    letters += "T";
                }
                else
                {
                    letters += "Y";
                }
            }
            else if (_easting >= 300000)
            {
                _easting -= 200000;
                if (_northing >= 400000)
                {
                    _northing -= 400000;
                    letters += "C";
                }
                else if (_northing >= 300000)
                {
                    _northing -= 300000;
                    letters += "H";
                }
                else if (_northing >= 200000)
                {
                    _northing -= 200000;
                    letters += "N";
                }
                else if (_northing >= 100000)
                {
                    _northing -= 100000;
                    letters += "S";
                }
                else
                {
                    letters += "X";
                }
            }
            else if (_easting >= 100000)
            {
                _easting -= 100000;
                if (_northing >= 400000)
                {
                    _northing -= 400000;
                    letters += "B";
                }
                else if (_northing >= 300000)
                {
                    _northing -= 300000;
                    letters += "G";
                }
                else if (_northing >= 200000)
                {
                    _northing -= 200000;
                    letters += "M";
                }
                else if (_northing >= 100000)
                {
                    _northing -= 100000;
                    letters += "R";
                }
                else
                {
                    letters += "W";
                }
            }
            else
            {
                if (_northing >= 400000)
                {
                    _northing -= 400000;
                    letters += "A";
                }
                else if (_northing >= 300000)
                {
                    _northing -= 300000;
                    letters += "F";
                }
                else if (_northing >= 200000)
                {
                    _northing -= 200000;
                    letters += "L";
                }
                else if (_northing >= 100000)
                {
                    _northing -= 100000;
                    letters += "Q";
                }
                else
                {
                    letters += "V";
                }
            }
            #endregion

            eastNumber = _easting.ToString().PadRight(5, '0');
            northNumber = _northing.ToString().PadRight(5, '0');

            while (eastNumber.Length > 2)
            {
                if (!eastNumber.EndsWith("0") || !northNumber.EndsWith("0"))
                {
                    break;
                }
                eastNumber = eastNumber.Remove(eastNumber.Length - 1);
                northNumber = northNumber.Remove(northNumber.Length - 1);
            }
        }
    }
}
