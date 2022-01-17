using System;
using System.IO;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace AuTan.ImageProcessing;

public class Card
{
    private static readonly FontCollection Fonts;
    private static readonly Color DarkPink = new (new Argb32(228, 122, 198, 255));

    static Card()
    {
        Fonts = new FontCollection();
        Fonts.Install(Path.Join(AppDomain.CurrentDomain.BaseDirectory,
            "./resources/card/Barlow-Regular.ttf"));
        Fonts.Install(Path.Join(AppDomain.CurrentDomain.BaseDirectory,
            "./resources/card/SecularOne-Regular.ttf"));
    }
    
    public static Image GetCardImage(string name, int level)
    {
        var img = Image.Load(Path.Join(AppDomain.CurrentDomain.BaseDirectory,
            "./resources/card/mockup.png"));
        var titleFont = Fonts.CreateFont("Secular One", 54, FontStyle.Bold);
        var bodyFont = Fonts.CreateFont("Barlow", 36, FontStyle.Bold);
        
        img.Mutate(x =>
        {
            x.DrawText(name, titleFont, DarkPink, new PointF(40, 420));
            x.DrawText($"Lv {level}", bodyFont, Color.White, new PointF(40, 500));
        });
        return img;
    }
}