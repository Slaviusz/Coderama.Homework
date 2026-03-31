namespace Coderama.Api.Mappers;

using Abstractions.Contracts.Responses;
using Abstractions.Models;
using Riok.Mapperly.Abstractions;
using System.Diagnostics.CodeAnalysis;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
[Mapper]
public partial class DocumentGetByIdMapper {
    [MapProperty(nameof(InternalDocument.InternalId), nameof(DocumentGetByIdResponse.Id))]
    public partial DocumentGetByIdResponse MapToResponse(InternalDocument internalDocument);
}
