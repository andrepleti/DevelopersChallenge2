using DevelopersChallenge2.Web.Data.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;

namespace DevelopersChallenge2.Web.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<DevelopersChallenge2Context>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }
    }
}