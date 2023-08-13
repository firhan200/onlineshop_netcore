using Data;
using Schema;

namespace Repositories.ImageRepository
{
    public class ImageRepository : GenericRepository<Image>
    {
        public ImageRepository(DataContext dataContext) : base(dataContext)
        {
        }
    }
}