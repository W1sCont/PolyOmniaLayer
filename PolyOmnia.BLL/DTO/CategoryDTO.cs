using PolyOmnia.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PolyOmnia.BLL.DTO
{
    public class CategoryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<VideoDTO> Videos { get; set; } = new List<VideoDTO>();
    }
}
