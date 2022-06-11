using HGV.Basilius.Client;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace HGV.Hyperstone.Images
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var httpClient = new HttpClient();
            var metaClient = new MetaClient();
            var heroes = metaClient.GetHeroes();
            var abilities = metaClient.GetAbilities();
            var items = metaClient.GetItems();
            var talents = heroes.SelectMany(_ => _.Talents).ToList();

            Directory.CreateDirectory(@"images\heroes\banner");
            Directory.CreateDirectory(@"images\heroes\profile");
            Directory.CreateDirectory(@"images\heroes\portrait");
            Directory.CreateDirectory(@"images\heroes\animation");
            Directory.CreateDirectory(@"images\abilities");
            Directory.CreateDirectory(@"images\items");
            
            foreach (var hero in heroes)
            {
                try
                {
                    var img = hero.Key.Replace("npc_dota_hero_", "");
                    var url = $"https://cdn.cloudflare.steamstatic.com/apps/dota2/videos/dota_react/heroes/renders/{img}.png";
                    
                    var stream = await httpClient.GetStreamAsync(url);
                    using (var fileStream = File.Create($@"images\heroes\profile\{hero.Id}.png"))
                    {
                        stream.CopyTo(fileStream);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to Download Hero profile {hero.Name} ");
                }
            }

            foreach (var hero in heroes)
            {
                try
                {
                    var img = hero.Key.Replace("npc_dota_hero_", "");
                    var url = $"https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/{img}.png";
                    var stream = await httpClient.GetStreamAsync(url);
                    using (var fileStream = File.Create($@"images\heroes\banner\{hero.Id}.png"))
                    {
                        stream.CopyTo(fileStream);
                    }
                }
                catch (Exception)
                {
                     Console.WriteLine($"Failed to Download Hero banner {hero.Key} ");
                }
            }

            foreach (var hero in heroes)
            {
                try
                {
                    var img = hero.Key.Replace("npc_dota_hero_", "");
                    var url = $"http://cdn.dota2.com/apps/dota2/images/heroes/{img}_vert.jpg";
                    var stream = await httpClient.GetStreamAsync(url);
                    using (var fileStream = File.Create($@"images\heroes\portrait\{hero.Id}.png"))
                    {
                        stream.CopyTo(fileStream);
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine($"Failed to Download Hero portrait {hero.Key} ");
                }
            }

            foreach (var hero in heroes)
            {
                try
                {
                    var img = hero.Key.Replace("npc_dota_hero_", "");
                    var url = $"https://cdn.cloudflare.steamstatic.com/apps/dota2/videos/dota_react/heroes/renders/{img}.webm";
                    var stream = await httpClient.GetStreamAsync(url);
                    using (var fileStream = File.Create($@"images\heroes\animation\{hero.Id}.webm"))
                    {
                        stream.CopyTo(fileStream);
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine($"Failed to Download Hero animation {hero.Key} ");
                }
            }

            foreach (var ability in abilities)
            {
                try
                {
                    if (ability.Key.Contains("special_bonus"))
                    {
                        var talent = talents.FirstOrDefault(_ => _.Key == ability.Key);
                        var lvl = talent?.Level ?? 0;
                        if (lvl == 0)
                            File.Copy($@"images\talents\talent_tree.png", $@"images\abilities\{ability.Id}.png", true);
                        else
                            File.Copy($@"images\talents\talent_tree_{lvl}.png", $@"images\abilities\{ability.Id}.png", true);
                    }
                    else
                    {
                        var img = ability.Key;
                        var url = $"https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/abilities/{img}.png";
                        var stream = await httpClient.GetStreamAsync(url);
                        using (var fileStream = File.Create($@"images\abilities\{ability.Id}.png"))
                        {
                            stream.CopyTo(fileStream);
                        }
                    }
                }
                catch (Exception ex)
                {
                     Console.WriteLine($"Failed to Download Ability {ability.Key}");
                }
            }

            foreach (var item in items)
            {
                try
                {
                    var img = item.Key.Replace("item_", "");
                    var url = $"https://cdn.cloudflare.steamstatic.com/apps/dota2/images/items/{img}_lg.png";
                    var stream = await httpClient.GetStreamAsync(url);
                    using (var fileStream = File.Create($@"images\items\{item.Id}.png"))
                    {
                        stream.CopyTo(fileStream);
                    }
                }
                catch (Exception)
                {
                     Console.WriteLine($"Failed to Download Item {item.Key}");
                }
            }
        }
    }
}
