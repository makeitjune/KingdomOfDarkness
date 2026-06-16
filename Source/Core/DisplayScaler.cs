using System;
using Microsoft.Xna.Framework;

namespace KingdomOfDarkness.Core;

public sealed class DisplayScaler
{
    public int VirtualWidth { get; }
    public int VirtualHeight { get; }

    public Rectangle DestinationRectangle { get; private set; }
    public float Scale { get; private set; }

    public DisplayScaler(int virtualWidth, int virtualHeight)
    {
        VirtualWidth = virtualWidth;
        VirtualHeight = virtualHeight;
        DestinationRectangle = new Rectangle(0, 0, virtualWidth, virtualHeight);
        Scale = 1f;
    }

    public void Update(int backBufferWidth, int backBufferHeight)
    {
        float scaleX = backBufferWidth / (float)VirtualWidth;
        float scaleY = backBufferHeight / (float)VirtualHeight;

        Scale = MathF.Min(scaleX, scaleY);

        int width = (int)MathF.Floor(VirtualWidth * Scale);
        int height = (int)MathF.Floor(VirtualHeight * Scale);

        int x = (backBufferWidth - width) / 2;
        int y = (backBufferHeight - height) / 2;

        DestinationRectangle = new Rectangle(x, y, width, height);
    }

    public Vector2 ActualToVirtual(Vector2 actual)
    {
        float x = (actual.X - DestinationRectangle.X) / Scale;
        float y = (actual.Y - DestinationRectangle.Y) / Scale;
        return new Vector2(x, y);
    }

    public bool ContainsActualPoint(Point point)
    {
        return DestinationRectangle.Contains(point);
    }
}
