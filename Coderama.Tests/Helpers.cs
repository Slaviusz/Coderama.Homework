namespace Coderama.Tests;

using Abstractions.Models;

public static class Helpers {
    private static readonly string[] AttrNames = ["name", "path", "user", "comment", "source", "type"];
    private static readonly char[] Charset = "abcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();

    public static InternalDocument GetFakeDocument()
    {
        return new(
        Guid.NewGuid().ToString("D"),
        [],
        JsonDocument.Parse(
        $$"""
          {
            "{{AttrNames[Random.Shared.Next(AttrNames.Length)]}}": "{{new(Random.Shared.GetItems(Charset, 8))}}",
            "{{AttrNames[Random.Shared.Next(AttrNames.Length)]}}": "{{new(Random.Shared.GetItems(Charset, 8))}}"
          }
          """)
        );
    }
}
