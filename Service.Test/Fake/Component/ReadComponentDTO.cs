using Quicker.Abstracts.Component;
using System;
using Test.Common.Repository;
using Test.Common.Repository.DTO;

namespace Quicker.Service.Test.Fake.Component
{
    public class ReadComponentDTO : ReadComponent<int, Product, ProductDTO>
    {
        public ReadComponentDTO(IServiceProvider service) : 
            base(service) { }
    }
}
