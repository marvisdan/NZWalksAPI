using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NZWalksAPI.Models.Domain;

namespace NZWalksAPI.Repositories
{
    public interface IImageRepository
    {
        Task<Image> Upload(Image image);
    }
}