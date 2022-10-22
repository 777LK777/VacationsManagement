namespace VacationService.DB.Migrations;

public class ScriptFile
{
    public string Name { get; set; }
    public string Content { get; set; }

    public override string ToString()
    {
        return Name;
    }
}