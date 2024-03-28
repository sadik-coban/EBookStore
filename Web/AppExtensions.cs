using Core.Entities;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Core.Data;
using Core.Infrastructure.Authentication;
using Microsoft.EntityFrameworkCore;


namespace Web;


public static class AppExtensions
{
    public static IApplicationBuilder SeedIdentity(this IApplicationBuilder builder)
    {
        using var scope = builder.ApplicationServices.CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

        context.Database.Migrate();
        try
        {
            roleManager.CreateAsync(new ApplicationRole { Name = "Administrators" }).Wait();
            roleManager.CreateAsync(new ApplicationRole { Name = "Members" }).Wait();

            var user = new Manager
            {
                Id = Guid.Parse("55D69A8A-8212-4D6D-A04B-199907AE983B"),
                UserName = configuration.GetValue<string>("Security:DefaultUser:UserName"),
                Email = configuration.GetValue<string>("Security:DefaultUser:UserName"),
                Name = configuration.GetValue<string>("Security:DefaultUser:Name"),
                EmailConfirmed = true
            };

            userManager.CreateAsync(user, configuration.GetValue<string>("Security:DefaultUser:Password")).Wait();
            userManager.AddToRoleAsync(user, "Administrators").Wait();
            var claimResult = userManager.AddClaimAsync(user, new Claim(ClaimTypes.GivenName, configuration.GetValue<string>("Security:DefaultUser:Name"))).Result;

            var customer = new Customer
            {
                Id = Guid.Parse("DC7EBFBE-82CF-46C8-8CA3-6F0093336981"),
                UserName = "user@mail.com",
                Email = "user@mail.com",
                Name = "User XYZ",
                EmailConfirmed = true
            };

            userManager.CreateAsync(customer, configuration.GetValue<string>("Security:DefaultUser:Password")).Wait();
            userManager.AddToRoleAsync(customer, "Members").Wait();
            var claimResultCustomer = userManager.AddClaimAsync(customer, new Claim(ClaimTypes.GivenName, "User XYZ")).Result;

            var userFound = userManager.FindByEmailAsync(configuration.GetValue<string>("Security:DefaultUser:UserName")).Result;
            var customerFound = userManager.FindByEmailAsync("user@mail.com").Result;

            context.CustomerAddresses.AddRange(new CustomerAddress
            {
                Customer = (Customer)customerFound,
                Name = "Ev",
                Text = "Apt. 833 8365 Emanuel Forest, Yundtville, NV 08212-8391"
            },
            new CustomerAddress
            {
                Customer = (Customer)customerFound,
                Name = "İş",
                Text = "527 Ernestine Port, Lake Trevor, OH 56158"
            });

            context.Publishers.AddRange(new Publisher
            {
                Id = Guid.Parse("EB05A239-BAC4-4E67-A7D9-FBE3D03096B2"),
                Name = "Kronik",
                DateCreated = DateTime.UtcNow,
                User = userFound,
                Enabled = true
            });

            var p1 = context.Publishers.Find(Guid.Parse("EB05A239-BAC4-4E67-A7D9-FBE3D03096B2"));
            context.Catalogs.AddRange(new Catalog
            {
                Id = Guid.Parse("93E198CC-0786-4BA3-8A4B-6E695E23B88C"),
                Name = "Sağlık",
                DateCreated = DateTime.UtcNow,
                User = userFound,
                Enabled = true
            }, new Catalog
            {
                Id = Guid.Parse("BA92D966-D043-4516-98D8-C55D51279811"),
                Name = "Fitness ve Beslenme",
                DateCreated = DateTime.UtcNow,
                User = userFound,
                Enabled = true
            });
            var c1 = context.Catalogs.Find(Guid.Parse("93E198CC-0786-4BA3-8A4B-6E695E23B88C"));
            var c2 = context.Catalogs.Find(Guid.Parse("BA92D966-D043-4516-98D8-C55D51279811"));

            context.Authors.AddRange(new Author
            {
                Id = Guid.Parse("34065B56-12B7-4D75-B5D2-05A11E070AFB"),
                Name = "Dr Ayşegül Çoruhlu",
                DateCreated = DateTime.UtcNow,
                User = userFound,
                Enabled = true
            });
            var a1 = context.Authors.Find(Guid.Parse("34065B56-12B7-4D75-B5D2-05A11E070AFB"));

            context.Products.AddRange(new Product
            {
                Publisher = p1,
                Name = "Longevity Planı - Gençleşmek İsteyenlerin El Kitabı",
                Price = 79.75m,
                Image = "https://m.media-amazon.com/images/I/61nlmp9zxgL._SY385_.jpg",
                DiscountRate = 10,
                Catalogs = new List<Catalog> { c1, c2 },
                Authors = new List<Author> { a1 },
                DateCreated = DateTime.UtcNow,
                User = userFound,
                Description = "BİLİMSEL YÖNTEMLER IŞIĞINDA\r\nYAŞLANMAYA SON!\r\n\r\n    • Gerçekten yaşlanmak zorunda mıyız? Yaşlanmak kaderimiz mi?\r\n    • Peki, neden yaşlanırız?\r\n    • Yaşlanmak aslında bir hastalık mı? Yaşlanmanın önüne geçilebilir mi?\r\n    • Peki, hangi özelliklere sahip bir hücre yaşlıdır, hangisi gençtir?\r\n    • Gençlik dönemini uzatmak mümkün mü? 140 yıl yaşamak ister miydiniz?\r\n\r\nTüm insanlar Gılgamış Destanı’ndan bu yana ölümsüzlüğü kovalamış, güç sahibi kişiler bu amaca ulaşmak için çaba sarf etmişler. Mumyalanmış firavunlar bu arzunun en güzel antik örneklerinden... \r\nElbette kimse yüz yaşına kadar yaşayıp da son yirmi yılını ah’layıp vah’layarak geçirmek istemez. Size bir haberim var; şu andan itibaren aşina olduğumuz dedelik-nineliğin modası geçti. Bizler öyle yaşlanmayacağız. \r\nAslında sıkı durun, bu kitabı okuyan hiç kimse eski usul yaşlanmayacağını öğrenecek.\r\nBir haberim daha var; bilim insanları geriye doğru gençleşmenin de mümkün olabileceğini söylüyorlar. \r\nİşte Longevity Planı, o zamana en az zararla, en sağlıklı hâlinizle ulaşmak, o güne olabilen en genç hücrelerle varabilmek için rehberiniz olacak. Biyolojik olarak yerinde saymaya başladığınızda şu anki durumunuzdan daha iyi bir hâlde olmanın yollarını gösterecek.\r\n\r\nLongevity adı verilen bu yeni tıbbın hedefi, sizi hastalıklarınızın henüz başlamadığı gençlikteki sağlığınıza döndürmektir.  \r\nUnutmayın; yaşlanma bir hastalıktır, tedavi edilebilir, durdurulabilir.\r\nBu kitap sizi yaşlanma hastalığından kurtaracak. Yaşam sürenizi uzatma rehberiniz olacak.\r\n\r\nYıllarını bu alana vermiş Dr. Ayşegül Çoruhlu, uzun zamandır beklenen bu kitabıyla, bilimsel yöntemlerin ışığında sizlere uzun sağlıklı bir yaşamın mümkün olduğunu ve bunun için yapmanız gerekenleri, yaş alma ile yaşlanma arasındaki farkı, üstelik bu kitapta öğreneceğiniz yöntemler sayesinde zamana karşı bir savaşla yaşlanmak yerine gençleşebileceğinizi anlatıyor. \r\nLongevity Planı, gençleşmek isteyenlerin el kitabı… \r\n\r\nHazırlanın, filmi geri sarıyoruz!",
                Enabled = true
            });

            context.SaveChanges();
        }
        catch
        {
            return builder;
        }


        return builder;
    }
}
