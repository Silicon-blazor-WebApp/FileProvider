﻿
using System.ComponentModel.DataAnnotations;

namespace Data.Entites
{
    public class FileEntity
    {
        [Key]
        public string FileName { get; set; } = null!;
        public string FilePath { get; set; } = null!;
        public string? ContentType { get; set; }
        public string? ContainerName { get; set; }
    }
}
