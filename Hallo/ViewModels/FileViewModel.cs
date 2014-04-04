using HalloDal.Models.Content;

namespace Hallo.ViewModels {
    public class FileViewModel {
        public FileViewModel(HalloFile file) {
            Id = file.Id;
            Description = file.Description;
            Extension = file.Extension;
        }
        public int Id { get; set; }
        public string Description { get; set; }
        public string Extension { get; set; }
        
        public string Url { get {
            return @"<a href='/Files/file" + Id + "." + Extension + "'>" + (Description != null ? Description : "file" + Id) + "</a>";
        } }
    }
}