using System.IO;
using System.Threading.Tasks;
using HandlebarsDotNet;

namespace api.Infrastructure.Services;
public class TemplateService
{
    public async Task<string> RenderTemplateAsync(string templatePath, object model)
    {
        var templateContent = await File.ReadAllTextAsync(templatePath);
        var template = Handlebars.Compile(templateContent);
        return template(model);
    }

    public async Task<string> LoadTemplateAsync(string templatePath)
    {
        return await File.ReadAllTextAsync(templatePath);
    }
}
