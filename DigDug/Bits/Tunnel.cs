using System.Drawing;
using Core;
using Core.Display;
using DigDug.Bits;

public class Tunnel : IGameElement
{
    private List<Rectangle> tunnelSegments;

    public Tunnel()
    {
        tunnelSegments = new List<Rectangle>();
    }

    public void Update(Player player)
    {
        // Add a tunnel segment at the player's current position
        tunnelSegments.Add(new Rectangle(player.X - Constants.TunnelWidth / 2, player.Y - Constants.TunnelWidth / 2, Constants.TunnelWidth, Constants.TunnelWidth));
    }

    public void Update()
    {
    }

    public void Draw(IDisplay display)
    {
        // Draw tunnel segments
        foreach (var segment in tunnelSegments)
        {
            display.DrawRectangle(segment.X, segment.Y, segment.Width, segment.Height, Color.Gray, Color.Gray);
        }
    }
}