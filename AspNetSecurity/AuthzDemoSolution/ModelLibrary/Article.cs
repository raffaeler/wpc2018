using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ModelLibrary
{
    [DebuggerDisplay("Name:{Name} State:{State} Maturity:{Maturity}")]
    public class Article
    {
        public Article()
        {
        }

        public static Article Create(string name, string owner)
        {
            return new Article()
            {
                Id = Guid.NewGuid(),
                Name = name,
                Owner = owner,
            };
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Owner { get; set; }
        public decimal Price { get; set; }
        public string Buyer { get; set; }


        public ArticleState State { get; set; }
        public Maturity Maturity { get; set; }
    }

}
