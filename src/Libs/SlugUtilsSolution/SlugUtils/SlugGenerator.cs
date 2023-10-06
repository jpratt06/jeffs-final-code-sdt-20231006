using Slugify;

namespace SlugUtils;

public class SlugGenerator
{


    public async Task<string> GenerateSlugAsync(string input, Func<string, Task<bool>> uniqueCheckerAsync)
    {
        var config = new SlugHelperConfiguration();

        var slugger = new SlugHelper(config);
        var attempt = slugger.GenerateSlug(input);
        var isUnique = await uniqueCheckerAsync(attempt);
        if (isUnique)
        {
            return attempt;
        }
        else
        {
            var letters = "abcdefghijklmnopqrstuvwxyz";
            var idx = 0;
            while (idx < letters.Length)
            {
                var retryAttempt = attempt + "-" + letters[idx];
                isUnique = await uniqueCheckerAsync(retryAttempt);
                if (isUnique)
                {
                    return retryAttempt;
                }
                else
                {
                    idx++;
                }

            }
        }
        return attempt + Guid.NewGuid();
    }
}