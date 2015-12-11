using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Geometry;

namespace Ocad.IO.Ocad9.Record.Helper
{
    internal partial class Setting
    {
        private const String TEMPLATE_OMEGA_ANGLE = "a";
        private const String TEMPLATE_PHI_ANGLE = "b";
        private const String TEMPLATE_DIM = "d";
        private const String TEMPLATE_RENDER_WITH_SPOT_COLOUR = "o";
        private const String TEMPLATE_ASSIGNED_TO_SPOT_COLOUR = "p";
        private const String TEMPLATE_SUBTRACT_FROM_SPOT_COLOUR = "q";
        private const String TEMPLATE_VISIBLE_IN_DRAFT_MODE = "r";
        private const String TEMPLATE_VISIBLE_IN_NORMAL_MODE = "s";
        private const String TEMPLATE_TRANSPARENT = "t";
        private const String TEMPLATE_OFFSET_CENTRE_X_MM = "x";
        private const String TEMPLATE_OFFSET_CENTRE_Y_MM = "y";
        private const String TEMPLATE_PIXEL_SIZE_X_MM = "u";
        private const String TEMPLATE_PIXEL_SIZE_Y_MM = "v";

        private void CopyToModelTemplate(Model.Map map)
        {
            Model.Template setting = new Model.Template();
            map.Templates.Add(setting);

            setting.FileName = _mainValue;

            int i = 0;
            while (i <= _codeValue.GetUpperBound(0))
            {
                string code = _codeValue[i, 0];
                switch (code)
                {
                    case TEMPLATE_OMEGA_ANGLE:
                        setting.OmegaAngle = GetDecimalValue(i);
                        break;
                    case TEMPLATE_PHI_ANGLE:
                        setting.PhiAngle = GetDecimalValue(i);
                        break;
                    case TEMPLATE_DIM:
                        setting.Dim = GetByteValue(i);
                        break;
                    case TEMPLATE_RENDER_WITH_SPOT_COLOUR:
                        setting.RenderWithSpotColour = GetStringValue(i);
                        break;
                    case TEMPLATE_ASSIGNED_TO_SPOT_COLOUR:
                        setting.AssignedToSpotColour = GetStringValue(i);
                        break;
                    case TEMPLATE_SUBTRACT_FROM_SPOT_COLOUR:
                        setting.SubtractFromSpotColour = GetStringValue(i);
                        break;
                    case TEMPLATE_VISIBLE_IN_DRAFT_MODE:
                        setting.VisibleInDraftMode = GetBooleanValue(i);
                        break;
                    case TEMPLATE_VISIBLE_IN_NORMAL_MODE:
                        setting.VisibleInNormalMode = GetBooleanValue(i);
                        break;
                    case TEMPLATE_TRANSPARENT:
                        setting.Transparent = GetStringValue(i);
                        break;
                    case TEMPLATE_OFFSET_CENTRE_X_MM:
                        setting.OffsetCentreX = GetDistance(i, Distance.Unit.Metre, Scale.milli);
                        break;
                    case TEMPLATE_OFFSET_CENTRE_Y_MM:
                        setting.OffsetCentreY = GetDistance(i, Distance.Unit.Metre, Scale.milli);
                        break;
                    case TEMPLATE_PIXEL_SIZE_X_MM:
                        setting.PixelSizeX = GetDistance(i, Distance.Unit.Metre, Scale.milli);
                        break;
                    case TEMPLATE_PIXEL_SIZE_Y_MM:
                        setting.PixelSizeY = GetDistance(i, Distance.Unit.Metre, Scale.milli);
                        break;
                    default:
                        throw CreateApplicationSettingException(i);
                }
                i++;
            }
        }

        private static void CopyFromModelTemplates(Model.Map map, List<Setting> settings)
        {
            foreach (Model.Template source in map.Templates)
            {
                Setting setting = new Setting();
                setting.SettingType = Type.SettingType.Template;
                settings.Add(setting);

                StringBuilder b = new StringBuilder(source.FileName);
                Write(b, TEMPLATE_VISIBLE_IN_NORMAL_MODE, source.VisibleInNormalMode);
                Write(b, TEMPLATE_VISIBLE_IN_DRAFT_MODE, source.VisibleInDraftMode);
                Write(b, TEMPLATE_PIXEL_SIZE_X_MM, source.PixelSizeX[10, Distance.Unit.Metre, Scale.milli], "0.0000000000");
                Write(b, TEMPLATE_PIXEL_SIZE_Y_MM, source.PixelSizeY[10, Distance.Unit.Metre, Scale.milli], "0.0000000000");
                Write(b, TEMPLATE_OFFSET_CENTRE_X_MM, source.OffsetCentreX[6, Distance.Unit.Metre, Scale.milli], "0.000000");
                Write(b, TEMPLATE_OFFSET_CENTRE_Y_MM, source.OffsetCentreY[6, Distance.Unit.Metre, Scale.milli], "0.000000");
                Write(b, TEMPLATE_OMEGA_ANGLE, source.OmegaAngle, "0.00000000");
                Write(b, TEMPLATE_PHI_ANGLE, source.PhiAngle, "0.00000000");
                Write(b, TEMPLATE_DIM, source.Dim);
                Write(b, TEMPLATE_RENDER_WITH_SPOT_COLOUR, source.RenderWithSpotColour);
                Write(b, TEMPLATE_ASSIGNED_TO_SPOT_COLOUR, source.AssignedToSpotColour);
                Write(b, TEMPLATE_SUBTRACT_FROM_SPOT_COLOUR, source.SubtractFromSpotColour);
                Write(b, TEMPLATE_TRANSPARENT, source.Transparent);
                setting.ConcatenatedValues = b.ToString();
            }
        }
    }
}
