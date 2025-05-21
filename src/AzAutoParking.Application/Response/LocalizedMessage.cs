namespace AzAutoParking.Application.Response;

public class LocalizedMessage(string en, string ptBr)
{
    public string En { get; set; } = en;
    public string PtBr { get; set; } = ptBr;
}