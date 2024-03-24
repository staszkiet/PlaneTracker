using Avalonia.Controls.Shapes;
using Avalonia.Rendering;
using FlightTrackerGUI;
using ObjectOrientedDesign;
using ObjectOrientedDesign.Objects;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text.Json;
public class Program
{
    public static void Main()
    {
        PlaneApp app = new PlaneApp();
        app.Run();
    }
}