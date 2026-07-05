using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Blish_HUD.Settings.UI.Views;
using Blish_HUD.Graphics.UI;
using System;
using Blish_HUD.Settings;


namespace Manlaan.MouseCursor.Views
{
    public class SettingsView : View
    {
        Panel colorPickerPanel;
        ColorPicker colorPicker;
        ColorBox colorBox;
        private static readonly Blish_HUD.Logger Logger = Blish_HUD.Logger.GetLogger<SettingsView>();

        protected override void Build(Container buildPanel)
        {
            Panel parentPanel = new Panel()
            {
                CanScroll = false,
                Parent = buildPanel,
                Height = buildPanel.Height,
                HeightSizingMode = SizingMode.AutoSize,
                Width = buildPanel.Width,  //bug? with buildPanel.Width changing to 40 after loading a different module settings and coming back.,
            };
            parentPanel.LeftMouseButtonPressed += delegate
            {
                if (colorPickerPanel.Visible && !colorPickerPanel.MouseOver && !colorBox.MouseOver)
                    colorPickerPanel.Visible = false;
            };

            colorPickerPanel = new Panel()
            {
                Location = new Point(parentPanel.Width - 420, 10),
                Size = new Point(420, 255),
                Visible = false,
                ZIndex = 10,
                Parent = parentPanel,
                BackgroundTexture = Module.ModuleInstance.ContentsManager.GetTexture("155976.png"),
                ShowBorder = false,
            };
            Panel colorPickerBG = new Panel()
            {
                Location = new Point(15, 15),
                Size = new Point(colorPickerPanel.Size.X - 35, colorPickerPanel.Size.Y - 30),
                Parent = colorPickerPanel,
                BackgroundTexture = Module.ModuleInstance.ContentsManager.GetTexture("buttondark.png"),
                ShowBorder = true,
            };
            colorPicker = new ColorPicker()
            {
                Location = new Point(10, 10),
                CanScroll = false,
                Size = new Point(colorPickerBG.Size.X - 20, colorPickerBG.Size.Y - 20),
                Parent = colorPickerBG,
                ShowTint = false,
                Visible = true
            };
            colorPicker.SelectedColorChanged += delegate
            {
                colorBox.Color = colorPicker.SelectedColor;
                Module._settingMouseCursorColor.Value = colorPicker.SelectedColor.Name;
                colorPickerPanel.Visible = false;
            };
            colorPicker.LeftMouseButtonPressed += delegate { colorPickerPanel.Visible = false; };
            foreach (var color in Module._colors) colorPicker.Colors.Add(color);


            Label cursorLabel = new Label()
            {
                Location = new Point(10, 15),
                Width = 60,
                AutoSizeHeight = false,
                WrapText = false,
                Parent = parentPanel,
                Text = "圖片: ",
            };
            Dropdown cursorSelect = new Dropdown()
            {
                Location = new Point(cursorLabel.Right + 5, cursorLabel.Top - 5),
                Width = 175,
                Parent = parentPanel,
            };
            foreach (var s in Module._mouseFiles) cursorSelect.Items.Add(s.Name);
            cursorSelect.SelectedItem = Module._settingMouseCursorImage.Value;
            cursorSelect.ValueChanged += delegate
            {
                Module._settingMouseCursorImage.Value = cursorSelect.SelectedItem;
            };
            colorBox = new ColorBox()
            {
                Location = new Point(cursorSelect.Right + 5, cursorSelect.Top),
                Parent = parentPanel,
                Size = new Point(cursorSelect.Bottom - cursorSelect.Top),
                Color = Module._colors.Find(x => x.Name.Equals(Module._settingMouseCursorColor.Value)),
            };
            colorBox.Click += delegate { colorPickerPanel.Visible = !colorPickerPanel.Visible; };
            Control prevContainer = cursorLabel;

            Label sizeLabel = new Label()
            {
                Location = new Point(10, prevContainer.Bottom + 5),
                Width = 60,
                AutoSizeHeight = false,
                WrapText = false,
                Parent = parentPanel,
                Text = "大小:",
            };
            TrackBar sizeSlider = new TrackBar()
            {
                Location = new Point(sizeLabel.Right + 5, sizeLabel.Top + 2),
                Width = 250,
                MaxValue = 250,
                MinValue = 0,
                Value = Module._settingMouseCursorSize.Value,
                Parent = parentPanel,
            };
            sizeSlider.ValueChanged += delegate { Module._settingMouseCursorSize.Value = (int)sizeSlider.Value; };
            prevContainer = sizeSlider;

            Label opacityLabel = new Label()
            {
                Location = new Point(10, prevContainer.Bottom + 5),
                Width = 60,
                AutoSizeHeight = false,
                WrapText = false,
                Parent = parentPanel,
                Text = "透明度:",
            };
            TrackBar opacitySlider = new TrackBar()
            {
                Location = new Point(opacityLabel.Right + 5, opacityLabel.Top + 2),
                Width = 250,
                MaxValue = 100,
                MinValue = 0,
                Value = Module._settingMouseCursorOpacity.Value * 100,
                Parent = parentPanel,
            };
            opacitySlider.ValueChanged += delegate { Module._settingMouseCursorOpacity.Value = opacitySlider.Value / 100; };
            prevContainer = opacityLabel;

            ViewContainer _settingAboveBlishContainer = new ViewContainer()
            {
                WidthSizingMode = SizingMode.Fill,
                Location = new Point(10, prevContainer.Bottom + 5),
                Parent = parentPanel
            };
            IView settingAboveBlishView = SettingView.FromType(Module._settingMouseCursorAboveBlish, _settingAboveBlishContainer.Width);
            _settingAboveBlishContainer.Show(settingAboveBlishView);
            prevContainer = _settingAboveBlishContainer;

            // 
            // 
            //                  Show Cursor Img and Clip Settings
            // 
            // 

            Label cursorClipShowHeader0Label = new Label()
            {
                Location = new Point(10, prevContainer.Bottom + 5),
                AutoSizeHeight = false,
                WrapText = false,
                Parent = parentPanel,
                Text = "",
                Width = 85,
            };
            Label cursorClipShowHeaderLabel = new Label()
            {
                Location = new Point(cursorClipShowHeader0Label.Right + 5, prevContainer.Bottom + 10),
                AutoSizeHeight = false,
                WrapText = false,
                Parent = parentPanel,
                Text = "戰鬥外",
                Width = 100,
            };
            Label cursorClipShowHeaderCombatLabel = new Label()
            {
                Location = new Point(cursorClipShowHeaderLabel.Right + 5, prevContainer.Bottom + 10),
                AutoSizeHeight = false,
                WrapText = false,
                Parent = parentPanel,
                Text = "戰鬥中",
                Width = 100,
            };
            prevContainer = cursorClipShowHeader0Label;

            Label cursorShowLabel = new Label()
            {
                Location = new Point(prevContainer.Left, prevContainer.Bottom + 10),
                AutoSizeHeight = false,
                WrapText = false,
                Parent = parentPanel,
                Text = "顯示圖片:",
                Width = prevContainer.Width,
            };
            Dropdown cursorShowSelect = new Dropdown()
            {
                Location = new Point(cursorClipShowHeaderLabel.Left, cursorClipShowHeaderLabel.Bottom + 5),
                Width = cursorClipShowHeaderLabel.Width,
                Parent = parentPanel,
            };
            foreach (Module.ShowMode mode in Enum.GetValues(typeof(Module.ShowMode)))
                cursorShowSelect.Items.Add(GetEnumDescription(mode));
            cursorShowSelect.SelectedItem = GetEnumDescription(Module._settingMouseCursorShow.Value);
            cursorShowSelect.ValueChanged += delegate
            {
                foreach (Module.ShowMode mode in Enum.GetValues(typeof(Module.ShowMode)))
                {
                    if (GetEnumDescription(mode) == cursorShowSelect.SelectedItem)
                    {
                        Module._settingMouseCursorShow.Value = mode;
                        break;
                    }
                }
            };
            Dropdown cursorShowCombatSelect = new Dropdown()
            {
                Location = new Point(cursorClipShowHeaderCombatLabel.Left, cursorClipShowHeaderCombatLabel.Bottom + 5),
                Width = cursorClipShowHeaderCombatLabel.Width,
                Parent = parentPanel,
            };
            foreach (Module.ShowMode mode in Enum.GetValues(typeof(Module.ShowMode)))
                cursorShowCombatSelect.Items.Add(GetEnumDescription(mode));
            cursorShowCombatSelect.SelectedItem = GetEnumDescription(Module._settingMouseCursorShowCombat.Value);
            cursorShowCombatSelect.ValueChanged += delegate
            {
                foreach (Module.ShowMode mode in Enum.GetValues(typeof(Module.ShowMode)))
                {
                    if (GetEnumDescription(mode) == cursorShowCombatSelect.SelectedItem)
                    {
                        Module._settingMouseCursorShowCombat.Value = mode;
                        break;
                    }
                }
            };
            prevContainer = cursorShowLabel;

            Label cursorClipLabel = new Label()
            {
                Location = new Point(prevContainer.Left, prevContainer.Bottom + 10),
                AutoSizeHeight = false,
                WrapText = false,
                Parent = parentPanel,
                Text = "限制游標:",
                Width = prevContainer.Width,
            };
            Dropdown cursorClipSelect = new Dropdown()
            {
                Location = new Point(cursorShowSelect.Left, cursorShowSelect.Bottom + 5),
                Width = cursorShowSelect.Width,
                Parent = parentPanel,
            };
            foreach (Module.ClipMode mode in Enum.GetValues(typeof(Module.ClipMode)))
                cursorClipSelect.Items.Add(GetEnumDescription(mode));
            cursorClipSelect.SelectedItem = GetEnumDescription(Module._settingMouseCursorClip.Value);
            cursorClipSelect.ValueChanged += delegate
            {
                foreach (Module.ClipMode mode in Enum.GetValues(typeof(Module.ClipMode)))
                {
                    if (GetEnumDescription(mode) == cursorClipSelect.SelectedItem)
                    {
                        Module._settingMouseCursorClip.Value = mode;
                        break;
                    }
                }
            };
            Dropdown cursorClipCombatSelect = new Dropdown()
            {
                Location = new Point(cursorShowCombatSelect.Left, cursorShowCombatSelect.Bottom + 5),
                Width = cursorShowCombatSelect.Width,
                Parent = parentPanel,
            };
            foreach (Module.ClipMode mode in Enum.GetValues(typeof(Module.ClipMode)))
                cursorClipCombatSelect.Items.Add(GetEnumDescription(mode));
            cursorClipCombatSelect.SelectedItem = GetEnumDescription(Module._settingMouseCursorClipCombat.Value);
            cursorClipCombatSelect.ValueChanged += delegate
            {
                foreach (Module.ClipMode mode in Enum.GetValues(typeof(Module.ClipMode)))
                {
                    if (GetEnumDescription(mode) == cursorClipCombatSelect.SelectedItem)
                    {
                        Module._settingMouseCursorClipCombat.Value = mode;
                        break;
                    }
                }
            };
            prevContainer = cursorClipCombatSelect;

            //
            //
            //                       Freeeze Cursor after Drag or Actioncam Settings
            //
            //

            ViewContainer _settingFreezeCursorContainer = new ViewContainer()
            {
                Location = new Point(10, prevContainer.Bottom + 5),
                Width = 210,
                Parent = parentPanel
            };
            IView settingFreezeCursorView = SettingView.FromType(Module._settingMouseCursorFreezeCursor, _settingFreezeCursorContainer.Width);
            _settingFreezeCursorContainer.Show(settingFreezeCursorView);

            TrackBar freezeCursorPeriodSlider = new TrackBar()
            {
                Location = new Point(_settingFreezeCursorContainer.Right + 5, _settingFreezeCursorContainer.Top + 5),
                Width = 250,
                MinValue = 1.0f,
                MaxValue = 500f,
                Value = Module._settingMouseCursorFreezeCursorPeriod.Value,
                BasicTooltipText = $"{Module._settingMouseCursorFreezeCursorPeriod.Value:0} 毫秒",
                Visible = Module._settingMouseCursorFreezeCursor.Value,
                Parent = parentPanel,
            };
            freezeCursorPeriodSlider.ValueChanged += delegate
            {
                Module._settingMouseCursorFreezeCursorPeriod.Value = freezeCursorPeriodSlider.Value;
                freezeCursorPeriodSlider.BasicTooltipText = $"{freezeCursorPeriodSlider.Value:0} 毫秒";
            };
            (settingFreezeCursorView as BoolSettingView).ValueChanged += delegate (object s, Blish_HUD.ValueEventArgs<bool> e)
            {
                freezeCursorPeriodSlider.Visible = Module._settingMouseCursorFreezeCursor.Value;
            };
            prevContainer = freezeCursorPeriodSlider;

            //
            //
            //                       Log Debug messages
            //
            //

            ViewContainer _settingLogDebugContainer = new ViewContainer()
            {
                WidthSizingMode = SizingMode.Fill,
                Location = new Point(10, prevContainer.Bottom + 5),
                Parent = parentPanel
            };
            IView settingLogDebugView = SettingView.FromType(Module._settingMouseCursorLogDebug, _settingLogDebugContainer.Width);
            _settingLogDebugContainer.Show(settingLogDebugView);
            prevContainer = _settingLogDebugContainer;
        }

        private static string GetEnumDescription(Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = (System.ComponentModel.DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(System.ComponentModel.DescriptionAttribute));
            return attribute == null ? value.ToString() : attribute.Description;
        }
    }
}
