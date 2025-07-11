namespace AzAutoParking.Application.Response;

public static class ErrorMessages
{
    public static class Auth
    {
        public static readonly LocalizedMessage InvalidEmailOrPassword =
            new("Email/Password invalid", "E-mail ou senha inválidos");

        public static readonly LocalizedMessage UnconfirmedAccount =
            new("Verify email. Sent an email for confirmation of your account.",
                "Verifique seu e-mail. Um e-mail foi enviado para confirmação da sua conta.");

        public static readonly LocalizedMessage InvalidCode = new("Invalid code", "Código inválido");
        public static readonly LocalizedMessage UserNotFound = new("User not found", "Usuário não encontrado");

        public static readonly LocalizedMessage CodeAlreadySent =
            new("Verify email. Sent an email for confirmation of code.",
                "Verifique seu e-mail. Um código de confirmação foi enviado.");

        public static readonly LocalizedMessage OldPasswordInvalid =
            new("Old Password invalid", "Senha antiga inválida");
        
        public static readonly LocalizedMessage EmailAlreadyExists = 
            new("Email already exists", "Email já cadastrado");
        
        public static readonly LocalizedMessage RequiredFullName =
            new ("Full name is required", "Nome completo é obrigatório");
    }

    public static class Parking
    {
        public static readonly LocalizedMessage NotFound = new("Parking not found", "Vaga não encontrada");

        public static readonly LocalizedMessage ParkingNumberExists =
            new("Parking number already exists", "Número da vaga já existe");
    }

    public static class System
    {
        public static readonly LocalizedMessage InternalError = new (en: "An unexpected error occurred. Please contact support", ptBr: "Ocorreu um erro inesperado. Entre em contato com o suporte");
        public static readonly LocalizedMessage RequiredEmail = new (en: "Email is required", ptBr: "Email completo é obrigatório");
        public static readonly LocalizedMessage InvalidEmail = new (en: "Invalid email", ptBr: "Email inválido");
        public static readonly LocalizedMessage RequiredPassword = new (en: "Password is required", ptBr: "Senha é obrigatório");
        public static readonly LocalizedMessage LongPassword = new (en: "Password must be at least 8 characters long", ptBr: "A senha deve ter pelo menos 8 caracteres");
        public static readonly LocalizedMessage NotMatchConfirmPassword = new (en: "Password does not match confirm password fields", ptBr: "A senha não corresponde aos campos de confirmação de senha");
    }
}