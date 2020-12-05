using System;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Customers.Models
{
    public class City
    {
        private ILazyLoader LazyLoader { get; set; }
        private Region _region;

        public City(ILazyLoader lazyLoader){
            LazyLoader = lazyLoader;
        }
        public City(){
        }
        public int Id {get ; set; }
        public string Name {get ; set; }
        public Region Region {get => LazyLoader.Load(this, ref _region) ; set => _region = value; }
    }

}