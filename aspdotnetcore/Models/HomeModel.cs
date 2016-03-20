using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.OptionsModel;

using Test.ViewModels;

namespace Test.Models
{
    public class HomeModel : IHomeModel
    {
        private IOptions<ConfigModel> _config;
        public HomeModel(IOptions<ConfigModel> config)
        {
            _config = config;
        }

        public HomeVM GetHomeVM()
        {
            return new HomeVM()
            {
                DateTime = DateTime.Now.Ticks.ToString(),
                Name = $"{ _config.Value.Option1} {_config.Value.Option2}"
            };
        }
    }
}
