using JWT.Algorithms;
using JWT.Builder;

namespace BootShopASP.Models; 

public class LoginService {
    private MyContext _myContext = new();
    const string _SECRET_ = "SUPERTAJNEHESLO";

    public bool Authenticate(mLogin model, out string token) {
        var admin = _myContext.tbAdmins.FirstOrDefault(x => x.login == model.Login);
        token = "";
        if (admin is null)
            return false;
        token = JwtBuilder.Create().WithAlgorithm(new HMACSHA256Algorithm()).WithSecret(_SECRET_).Encode();

        return BCrypt.Net.BCrypt.Verify(model.Password, admin.password);
    }

    public bool VerifyToken(string token) {
        try
        {
            _ = JwtBuilder.Create()
                .WithAlgorithm(new HMACSHA256Algorithm())
                .WithSecret(_SECRET_)
                .MustVerifySignature()
                .Decode(token);
            return true;
        }
        catch 
        {
            return false;
        }
    }
}