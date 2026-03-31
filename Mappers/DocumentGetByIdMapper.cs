namespace Coderama.Homework.Mappers;

using Riok.Mapperly.Abstractions;

[Mapper]
internal partial class DocumentGetByIdMapper {
    [MapProperty(nameof(InternalDocument.InternalId), nameof(DocumentGetByIdResponse.Id))]
    public partial DocumentGetByIdResponse MapToResponse(InternalDocument internalDocument);
}
