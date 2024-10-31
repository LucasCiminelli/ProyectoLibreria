using Microsoft.EntityFrameworkCore;
using AppStore.Models.Domain;
using Microsoft.AspNetCore.Identity;
using AppStore.Repositories.Abstract;
using AppStore.Repositories.Implementation;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<ILibroService, LibroService>(); //Cada vez que una clase que no tenga a ILibroService referenciado, lo generará automáticamente. Abstract e implementacion. 
                                                           //Cuando arranca el programa, existe la lógica para que cree automaticamente un objeto del tipo ILibroService 
                                                           //con su implementación LibroService para estár disponible.
builder.Services.AddScoped<IUserAuthenticationService, UserAuthenticationService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<ICategoriaService, CategoriaService>();

builder.Services.AddDbContext<DatabaseContext>(opt =>
{
    opt.LogTo(Console.WriteLine, new[] {
    DbLoggerCategory.Database.Command.Name},
            LogLevel.Information).EnableSensitiveDataLogging();

    opt.UseSqlite(builder.Configuration.GetConnectionString("SqliteDatabase"));

});

builder.Services.AddIdentity<AplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<DatabaseContext>()
            .AddDefaultTokenProviders();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); //para trabajar el login con token y cookies en sesión.
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using (var ambiente = app.Services.CreateScope()) //inserción data master.
{
    var services = ambiente.ServiceProvider;

    try
    {
        var context = services.GetRequiredService<DatabaseContext>();

        var userManager = services.GetRequiredService<UserManager<AplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();


        await context.Database.MigrateAsync(); //ejecuta archivos de migración existentes. va a buscar la carpeta migrations y va a ejecutar el codigo dentro. crea tablas en la db.
        await LoadDatabase.InsertarData(context, userManager, roleManager);
    }
    catch (Exception e)
    {
        var logging = services.GetRequiredService<ILogger<Program>>();
        logging.LogError(e, "Ocurrio un error en la inserción de datos.");
    }






}




app.Run();
