using API.Interfaces;

namespace API.Validation
{
    public static class RegexController
    {
        public const string regexName = @"^[[a-zA-ZçáéíóúâêôãõàòèìùñÇÁÉÍÓÚÂÊÔÃÕÀÒÈÌÙÑąęśćńłóżźĄĘŚĆŃŁÓŻŹă''-'\s]]{{1,50}}$";
        public const string regexCountry = @"^[[a-zA-Z''-'\s]]{{1,50}}$";
        public const string regexID = @"^[[0-9]]{{8,11}}[[A-Z]]{{0,1}}$";
    }
}
