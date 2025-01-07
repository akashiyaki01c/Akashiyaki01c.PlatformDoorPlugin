using System;
using System.Drawing;
using System.IO;
using BveEx.PluginHost;
using BveTypes.ClassWrappers;
using Zbx1425.DXDynamicTexture;

namespace Akashiyaki01c.PlatformDoorPlugin
{
    partial class PlatformDoor
    {
        private string BaseDirectory;

        private TextureHandle TextureHandle;
        private GDIHelper GDIHelper;

        private Bitmap NoneBitmap;
        private Bitmap OpenBitmap;
        private Bitmap CloseBitmap;

        private void OnScenarioCreatedStructure(ScenarioCreatedEventArgs e)
        {
            BaseDirectory = Path.GetDirectoryName(Location);
            NoneBitmap = new Bitmap(Image.FromFile(Path.Combine(BaseDirectory, Settings.NoneImagePath)));
            OpenBitmap = new Bitmap(Image.FromFile(Path.Combine(BaseDirectory, Settings.OpenImagePath)));
            CloseBitmap = new Bitmap(Image.FromFile(Path.Combine(BaseDirectory, Settings.CloseImagePath)));

            Model targetModel = e.Scenario.Map.StructureModels[Settings.DisplayStructureId];
            TextureHandle = targetModel.Register(Settings.DisplayStructureTextureFile);
            GDIHelper = new GDIHelper(TextureHandle.Width, TextureHandle.Height);
        }

        private void TickStructure(TimeSpan elapsed)
        {
            if (TextureHandle.HasEnoughTimePassed(10))
            {
                GDIHelper.BeginGDI();
                {

                }
                GDIHelper.EndGDI();
                TextureHandle.Update(GDIHelper);
            }
        }

        private void DrawTextureNone()
        {
            GDIHelper.BeginGDI();
            {
                GDIHelper.DrawImage(NoneBitmap, 0, 0);
            }
            GDIHelper.EndGDI();
            TextureHandle.Update(GDIHelper);
        }
        private void DrawTextureOpen()
        {
            GDIHelper.BeginGDI();
            {
                GDIHelper.DrawImage(OpenBitmap, 0, 0);
            }
            GDIHelper.EndGDI();
            TextureHandle.Update(GDIHelper);
        }
        private void DrawTextureClose()
        {
            GDIHelper.BeginGDI();
            {
                GDIHelper.DrawImage(CloseBitmap, 0, 0);
            }
            GDIHelper.EndGDI();
            TextureHandle.Update(GDIHelper);
        }
    }

}
