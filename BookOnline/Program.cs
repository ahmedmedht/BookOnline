using BookOnline.Services.imp;
using BookOnline.Services;
using BookOnline.Services.Imp;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(option =>
option.UseSqlServer(connectionString));
builder.Services.AddSwaggerGen();


builder.Services.AddTransient<IAuthorService, AuthorService>();
builder.Services.AddTransient<IBookDetailService, BookDetailService>();
builder.Services.AddTransient<IBookProductService, BookProductService>();
builder.Services.AddTransient<ICartService, CartService>();
builder.Services.AddTransient<IImageService, ImageService>();


builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
