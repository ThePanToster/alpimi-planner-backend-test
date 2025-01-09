using AlpimiAPI.Settings;

namespace AlpimiAPI.Utilities
{
    public static class CharacterFilter
    {
        public static bool Required(string str, RequiredCharacterTypes[]? requiredCharacterTypes)
        {
            if (requiredCharacterTypes == null)
            {
                return true;
            }

            if (requiredCharacterTypes.Contains(RequiredCharacterTypes.BigLetter))
            {
                if (!str.Any(char.IsUpper))
                {
                    return false;
                }
            }
            if (requiredCharacterTypes.Contains(RequiredCharacterTypes.SmallLetter))
            {
                if (!str.Any(char.IsLower))
                {
                    return false;
                }
            }
            if (requiredCharacterTypes.Contains(RequiredCharacterTypes.Digit))
            {
                if (!str.Any(char.IsDigit))
                {
                    return false;
                }
            }
            if (requiredCharacterTypes.Contains(RequiredCharacterTypes.Symbol))
            {
                if (!(str.Any(char.IsSymbol) || str.Any(char.IsPunctuation)))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool Allowed(string str, AllowedCharacterTypes[]? allowedCharacterTypes)
        {
            if (allowedCharacterTypes == null)
            {
                return false;
            }

            foreach (char letter in str)
            {
                if (
                    char.IsLower(letter)
                    && allowedCharacterTypes.Contains(AllowedCharacterTypes.SmallLetters)
                )
                {
                    continue;
                }

                if (
                    char.IsUpper(letter)
                    && allowedCharacterTypes.Contains(AllowedCharacterTypes.BigLetters)
                )
                {
                    continue;
                }

                if (
                    (char.IsSymbol(letter) || char.IsPunctuation(letter))
                    && allowedCharacterTypes.Contains(AllowedCharacterTypes.Symbols)
                )
                {
                    continue;
                }

                if (
                    char.IsDigit(letter)
                    && allowedCharacterTypes.Contains(AllowedCharacterTypes.Digits)
                )
                {
                    continue;
                }

                if (letter == ' ' && allowedCharacterTypes.Contains(AllowedCharacterTypes.Spaces))
                {
                    continue;
                }

                return false;
            }

            return true;
        }
    }
}
