using ServiceStack;
using ServiceStack.DataAnnotations;
using ServiceStack.Model;
using System.Collections.Generic;

namespace StackX.ServiceModel.Types
{
    public class Taxonomy : AuditBase, IHasIntId
    {
        [AutoIncrement] public int Id { get; set; }

        [Reference] public Application Application { get; set; }
        public int ApplicationId { get; set; }

        public string Name { get; set; }

        public string Slug { get; set; }

        public string Description { get; set; }

        public int? ParentId { get; set; }

        public Dictionary<string, string> Meta { get; set; }
    }
}