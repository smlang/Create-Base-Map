using System;
using System.Collections.Generic;
using System.Text;
using System.Management.Automation;
using System.IO;

namespace Ocad.Module
{
    [Cmdlet(VerbsData.Merge, "ColourTable")]
    public class MergeColourTable : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public Model.Map TargetMap;

        [Parameter(Position = 0, Mandatory = true)]
        public Model.Map SourceMap;

        protected override void ProcessRecord()
        {
            List<Model.SpotColour> newSpotColourTable = new List<Model.SpotColour>();
            List<Model.Colour> newColourTable = new List<Model.Colour>();

            #region Spot Colour
            int spotColourIndex = 1;
            foreach (Model.SpotColour newSpotColour in SourceMap.SpotColours)
            {
                Model.SpotColour newSpotColourClone = new Model.SpotColour();

                newSpotColourClone.Name = newSpotColour.Name;
                newSpotColourClone.Number = spotColourIndex;
                spotColourIndex++;
                newSpotColourClone.Cyan = newSpotColour.Cyan;
                newSpotColourClone.Magenta = newSpotColour.Magenta;
                newSpotColourClone.Yellow = newSpotColour.Yellow;
                newSpotColourClone.Black = newSpotColour.Black;
                newSpotColourClone.Visible = newSpotColour.Visible;
                newSpotColourClone.FrequencyLpi = newSpotColour.FrequencyLpi;
                newSpotColourClone.HalftoneAngle = newSpotColour.HalftoneAngle;

                newSpotColourTable.Add(newSpotColourClone);
            }
            foreach (Model.SpotColour oldSpotColour in TargetMap.SpotColours)
            {
                bool found = false;
                foreach (Model.SpotColour newSpotColour in SourceMap.SpotColours)
                {
                    if (oldSpotColour.Name == newSpotColour.Name)
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    oldSpotColour.Number = spotColourIndex;
                    spotColourIndex++;
                    newSpotColourTable.Add(oldSpotColour);
                }
            }
            #endregion

            #region Colour
            // Remove colours from this table as added to the new table
            List<Model.Colour> oldColourTable = new List<Model.Colour>();
            oldColourTable.AddRange(TargetMap.ColourTable);

            // Keep track of numbers used by colours added to the new table
            List<Int16> usedNumber = new List<Int16>();
            // Keep track of colours that need to be renumbered
            List<Model.Colour> coloursToBeRenumbered = new List<Model.Colour>();

            #region Add new colour to table
            foreach (Model.Colour newColour in SourceMap.ColourTable)
            {
                if (usedNumber.Contains(newColour.Number))
                {
                    coloursToBeRenumbered.Add(newColour);
                }
                else
                {
                    usedNumber.Add(newColour.Number);
                }

                #region Find match with old colour
                Model.Colour newColourClone = null;
                foreach (Model.Colour oldColour in oldColourTable)
                {
                    if ((oldColour.Number == newColour.Number) || (oldColour.Name == newColour.Name))
                    {
                        newColourClone = oldColour; // add but, not clone the old colour instance, so all references to it remain intact
                        break;
                    }
                }

                if (newColourClone == null)
                {
                    //newColourClone = new Model.Colour();
                }
                else
                {
                    oldColourTable.Remove(newColourClone);
                }
                #endregion

                newColourTable.Add(newColourClone);
                newColourClone.Number = newColour.Number;
                newColourClone.Name = newColour.Name;
                newColourClone.Cyan = newColour.Cyan;
                newColourClone.Magenta = newColour.Magenta;
                newColourClone.Yellow = newColour.Yellow;
                newColourClone.Black = newColour.Black;
                newColourClone.Overprint = newColour.Overprint;
                newColourClone.Transparency = newColour.Transparency;
                CloneColourSpotColours(newColourClone, newSpotColourTable);
            }
            #endregion

            #region Add old colour to table if no match with a new colour
            foreach (Model.Colour oldColour in oldColourTable)
            {
                if (usedNumber.Contains(oldColour.Number))
                {
                    coloursToBeRenumbered.Add(oldColour);
                }
                else
                {
                    usedNumber.Add(oldColour.Number);
                }

                newColourTable.Add(oldColour); // add but, not clone the old colour instance, so all references to it remain intact
                CloneColourSpotColours(oldColour, newSpotColourTable);
            }
            #endregion

            #region Renumber Colour sharing number with another colour
            Int16 newNumber = -1;
            foreach (Model.Colour changeColour in coloursToBeRenumbered)
            {
                while (true)
                {
                    newNumber++;
                    if (!usedNumber.Contains(newNumber))
                    {
                        changeColour.Number = newNumber;
                        break;
                    }
                }
            }
            #endregion
            #endregion

            TargetMap.SpotColours = newSpotColourTable;
            TargetMap.ColourTable = newColourTable;
        }

        private void CloneColourSpotColours(Model.Colour colour, List<Model.SpotColour> newSpotColourTable)
        {
            Dictionary<Model.SpotColour, Decimal> newColourSpotColourTable = new Dictionary<Model.SpotColour, Decimal>();
            foreach (Model.SpotColour spotColourKey in colour.SpotColours.Keys)
            {
                foreach (Model.SpotColour newSpotColourClone in newSpotColourTable)
                {
                    if (newSpotColourClone.Name == spotColourKey.Name)
                    {
                        newColourSpotColourTable.Add(newSpotColourClone, colour.SpotColours[spotColourKey]);
                    }
                }
            }
            colour.SpotColours = newColourSpotColourTable;
        }
    }
}
