using System.Linq;

namespace TestNinja.Mocking
{
    public interface IBookingRepository
    {
        IQueryable<Booking> FindActiveBookings(int excludedBookingId);
    }

    public class BookingRepository : IBookingRepository
    {
        public IQueryable<Booking> FindActiveBookings(int excludedBookingId)
        {
            var unitOfWork = new UnitOfWork();
            var bookings =
                unitOfWork.Query<Booking>()
                    .Where(
                        b => b.Id != excludedBookingId && b.Status != "Cancelled");

            return bookings;
        }
    }
}
