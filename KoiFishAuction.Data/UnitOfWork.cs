using KoiFishAuction.Data.Models;
using KoiFishAuction.Data.Repository;

namespace KoiFishAuction.Data
{
    public class UnitOfWork
    {
        private readonly FA24_NET1720_PRN231_G6_KoiFishAuctionContext _context;

        public UnitOfWork(FA24_NET1720_PRN231_G6_KoiFishAuctionContext context)
        {
            _context = context;
        }

        private UserRepository _userRepository;
        public UserRepository UserRepository
        {
            get
            {
                if (_userRepository == null)
                {
                    _userRepository = new UserRepository(_context);
                }
                return _userRepository;
            }
        }

        private KoiFishRepository _koiFishRepository;
        public KoiFishRepository KoiFishRepository
        {
            get
            {
                if (_koiFishRepository == null)
                {
                    _koiFishRepository = new KoiFishRepository(_context);
                }
                return _koiFishRepository;
            }
        }

        private AuctionSessionRepository _auctionSessionRepository;
        public AuctionSessionRepository AuctionSessionRepository
        {
            get
            {
                if (_auctionSessionRepository == null)
                {
                    _auctionSessionRepository = new AuctionSessionRepository(_context, this);
                }
                return _auctionSessionRepository;
            }
        }

        private BidRepository _bidRepository;
        public BidRepository BidRepository
        {
            get
            {
                if (_bidRepository == null)
                {
                    _bidRepository = new BidRepository(_context);
                }
                return _bidRepository;
            }
        }

        private AuctionHistoryRepository _auctionHistoryRepository;
        public AuctionHistoryRepository AuctionHistoryRepository
        {
            get
            {
                if (_auctionHistoryRepository == null)
                {
                    _auctionHistoryRepository = new AuctionHistoryRepository();
                }
                return _auctionHistoryRepository;
            }
        }

        private PaymentRepository _paymentRepository;
        public PaymentRepository PaymentRepository
        {
            get
            {
                if (_paymentRepository == null)
                {
                    _paymentRepository = new PaymentRepository();
                }
                return _paymentRepository;
            }
        }

        private NotificationRepository notificationRepository;
        public NotificationRepository NotificationRepository
        {
            get
            {
                if (notificationRepository == null)
                {
                    notificationRepository = new NotificationRepository();
                }
                return notificationRepository;
            }
        }
    }
}
