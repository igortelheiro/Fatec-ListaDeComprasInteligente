using ListaDeComprasInteligente.Scraper;
using ListaDeComprasInteligente.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<ListaComprasBuilderService>();
builder.Services.AddSingleton<HtmlParserService>();
builder.Services.AddSingleton<ScraperService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();