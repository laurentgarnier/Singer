using Singer.Business.Api.SongsManagement;
using Singer.LibrariesManagement.Module.Models;

namespace Singer.LibrariesManagement.Module.Extensions
{
    internal static class SongLibraryDtoExtension
    {
        public static SongLibraryModel ConvertToModel(this SongLibraryDto dto)
        {
            var model = new SongLibraryModel()
            {
                Id = dto.Id,
                Name = dto.Name,
            };

            FillWithChildLibraries(model, dto);
            return model;
        }

        private static void FillWithChildLibraries(SongLibraryModel model, SongLibraryDto dto)
        {
            foreach (var library in dto.ChildsLibraries)
            {
                var childSongLibraryModel = new SongLibraryModel()
                {
                    Id = library.Id,
                    Name = library.Name
                };
                model.Childs.Add(childSongLibraryModel);
                FillWithChildLibraries(childSongLibraryModel, library);
            }
        }

    }
}
