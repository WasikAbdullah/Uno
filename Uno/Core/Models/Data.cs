using System.Text.Json;
using Uno.Core.Enums;

namespace Uno.Core.Models;

public class Data
{
    public Actions Action { get; set; }
    public JsonElement Payload { get; set; }
}