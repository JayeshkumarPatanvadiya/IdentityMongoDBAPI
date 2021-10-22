using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityMongoDBAPI.Models
{
    [CollectionName("Users")]
    public class ApplicationsUsers : MongoIdentityUser<Guid>
    {

    }
}
