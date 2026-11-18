namespace DotNetProjectFile.IO;

public interface IOMatch
{
    bool Matches(Chars full, Chars local);
}
