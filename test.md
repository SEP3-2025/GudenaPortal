flowchart BT
AccountDetailsRepository(AccountDetailsRepository)
AccountDetailsService(AccountDetailsService)
AuthController(AuthController)
BasketController(BasketController)
BasketDto(BasketDto)
BasketExceedsStockException(BasketExceedsStockException)
BasketItemDto(BasketItemDto)
BasketRepository(BasketRepository)
BasketService(BasketService)
BusinessInvalidStatusChangeException(BusinessInvalidStatusChangeException)
BusinessOrderController(BusinessOrderController)
BusinessOrderDto(BusinessOrderDto)
BusinessOrderItemDto(BusinessOrderItemDto)
BusinessOrderRepository(BusinessOrderRepository)
BusinessOrderService(BusinessOrderService)
BusinessOwnershipException(BusinessOwnershipException)
BusinessProductController(BusinessProductController)
BusinessProductRepository(BusinessProductRepository)
BusinessProductReturnController(BusinessProductReturnController)
BusinessProductReturnDto(BusinessProductReturnDto)
BusinessProductReturnRepository(BusinessProductReturnRepository)
BusinessProductReturnService(BusinessProductReturnService)
BusinessProductService(BusinessProductService)
BusinessRegisterDto(BusinessRegisterDto)
BusinessWarrantyClaimController(BusinessWarrantyClaimController)
BusinessWarrantyClaimDto(BusinessWarrantyClaimDto)
BusinessWarrantyClaimRepository(BusinessWarrantyClaimRepository)
BusinessWarrantyClaimService(BusinessWarrantyClaimService)
BuyerProductService(BuyerProductService)
BuyerProductsController(BuyerProductsController)
BuyerRegisterDto(BuyerRegisterDto)
CannotModifyOrderException(CannotModifyOrderException)
CategoryController(CategoryController)
CategoryRepository(CategoryRepository)
CategoryService(CategoryService)
CreateShippingDto(CreateShippingDto)
CreditCardDto(CreditCardDto)
FavouriteController(FavouriteController)
FavouriteRepository(FavouriteRepository)
FavouriteService(FavouriteService)
GudenaPaymentClient(GudenaPaymentClient)
GudenaShippingClient(GudenaShippingClient)
IAccountDetailsRepository(IAccountDetailsRepository)
IAccountDetailsService(IAccountDetailsService)
IBasketRepository(IBasketRepository)
IBasketService(IBasketService)
IBusinessOrderRepository(IBusinessOrderRepository)
IBusinessOrderService(IBusinessOrderService)
IBusinessProductRepository(IBusinessProductRepository)
IBusinessProductReturnRepository(IBusinessProductReturnRepository)
IBusinessProductReturnService(IBusinessProductReturnService)
IBusinessProductService(IBusinessProductService)
IBusinessWarrantyClaimRepository(IBusinessWarrantyClaimRepository)
IBusinessWarrantyClaimService(IBusinessWarrantyClaimService)
IBuyerProductService(IBuyerProductService)
ICategoryRepository(ICategoryRepository)
ICategoryService(ICategoryService)
IFavouriteRepository(IFavouriteRepository)
IFavouriteService(IFavouriteService)
IOrderItemRepository(IOrderItemRepository)
IOrderItemService(IOrderItemService)
IOrderRepository(IOrderRepository)
IOrderService(IOrderService)
IPaymentRepository(IPaymentRepository)
IPaymentService(IPaymentService)
IProductRepository(IProductRepository)
IProductReturnRepository(IProductReturnRepository)
IProductReturnService(IProductReturnService)
IShippingRepository(IShippingRepository)
IShippingService(IShippingService)
IWarrantyClaimRepository(IWarrantyClaimRepository)
IWarrantyClaimService(IWarrantyClaimService)
IncancellableOrderException(IncancellableOrderException)
LocationsShippingsDto(LocationsShippingsDto)
LoginDto(LoginDto)
NonReturnableProductException(NonReturnableProductException)
OrderController(OrderController)
OrderExceedsStockException(OrderExceedsStockException)
OrderItemDto(OrderItemDto)
OrderItemRepository(OrderItemRepository)
OrderItemService(OrderItemService)
OrderRepository(OrderRepository)
OrderService(OrderService)
OrderShippingDto(OrderShippingDto)
OrderUpdateDto(OrderUpdateDto)
PaymentController(PaymentController)
PaymentDto(PaymentDto)
PaymentRepository(PaymentRepository)
PaymentService(PaymentService)
ProductAlreadyReturnedException(ProductAlreadyReturnedException)
ProductCreateDto(ProductCreateDto)
ProductRepository(ProductRepository)
ProductReturnController(ProductReturnController)
ProductReturnDto(ProductReturnDto)
ProductReturnRepository(ProductReturnRepository)
ProductReturnService(ProductReturnService)
ResourceNotFoundException(ResourceNotFoundException)
ShippingController(ShippingController)
ShippingRepository(ShippingRepository)
ShippingService(ShippingService)
UnpaidException(UnpaidException)
UpdateBusinessProductReturnStatusDto(UpdateBusinessProductReturnStatusDto)
UpdateBusinessWarrantyClaimStatusDto(UpdateBusinessWarrantyClaimStatusDto)
UpdateOrderStatusDto(UpdateOrderStatusDto)
UpdateShippingDto(UpdateShippingDto)
UserDoesNotOwnResourceException(UserDoesNotOwnResourceException)
UsersController(UsersController)
WarrantyAlreadyClaimedException(WarrantyAlreadyClaimedException)
WarrantyClaimController(WarrantyClaimController)
WarrantyClaimDto(WarrantyClaimDto)
WarrantyClaimRepository(WarrantyClaimRepository)
WarrantyClaimService(WarrantyClaimService)

AccountDetailsRepository  -..-|>  IAccountDetailsRepository 
AccountDetailsService  -->  IAccountDetailsRepository 
AccountDetailsService  -..->  IAccountDetailsRepository 
AccountDetailsService  --*  IAccountDetailsRepository 
AccountDetailsService  -..-|>  IAccountDetailsService 
AuthController  -->  BusinessRegisterDto 
AuthController  -->  BuyerRegisterDto 
AuthController  -->  LoginDto 
BasketController  -->  BasketExceedsStockException 
BasketController  -->  BasketItemDto 
BasketController  -->  IBasketService 
BasketController  -..->  IBasketService 
BasketController  --*  IBasketService 
BasketController  -->  ResourceNotFoundException 
BasketDto  -->  BasketItemDto 
BasketRepository  -->  BasketExceedsStockException 
BasketRepository  -..-|>  IBasketRepository 
BasketRepository  -->  IProductRepository 
BasketRepository  -..->  IProductRepository 
BasketRepository  --*  IProductRepository 
BasketRepository  -->  ResourceNotFoundException 
BasketService  -->  IBasketRepository 
BasketService  --*  IBasketRepository 
BasketService  -..->  IBasketRepository 
BasketService  -..-|>  IBasketService 
BusinessOrderController  -->  BusinessInvalidStatusChangeException 
BusinessOrderController  -->  BusinessOrderDto 
BusinessOrderController  -->  BusinessOwnershipException 
BusinessOrderController  --*  IBusinessOrderService 
BusinessOrderController  -..->  IBusinessOrderService 
BusinessOrderController  -->  IBusinessOrderService 
BusinessOrderController  -->  UpdateOrderStatusDto 
BusinessOrderDto  -->  BusinessOrderItemDto 
BusinessOrderRepository  -->  BusinessInvalidStatusChangeException 
BusinessOrderRepository  -->  BusinessOrderDto 
BusinessOrderRepository  -->  BusinessOrderItemDto 
BusinessOrderRepository  -->  BusinessOwnershipException 
BusinessOrderRepository  -..-|>  IBusinessOrderRepository 
BusinessOrderService  -->  BusinessOrderDto 
BusinessOrderService  -->  IBusinessOrderRepository 
BusinessOrderService  --*  IBusinessOrderRepository 
BusinessOrderService  -..->  IBusinessOrderRepository 
BusinessOrderService  -..-|>  IBusinessOrderService 
BusinessProductController  -->  IBusinessProductService 
BusinessProductController  -..->  IBusinessProductService 
BusinessProductController  --*  IBusinessProductService 
BusinessProductController  -->  ProductCreateDto 
BusinessProductRepository  -..-|>  IBusinessProductRepository 
BusinessProductReturnController  -->  BusinessProductReturnDto 
BusinessProductReturnController  -->  IBusinessProductReturnService 
BusinessProductReturnController  -..->  IBusinessProductReturnService 
BusinessProductReturnController  --*  IBusinessProductReturnService 
BusinessProductReturnController  -->  UpdateBusinessProductReturnStatusDto 
BusinessProductReturnRepository  -->  BusinessProductReturnDto 
BusinessProductReturnRepository  -..-|>  IBusinessProductReturnRepository 
BusinessProductReturnService  -->  BusinessProductReturnDto 
BusinessProductReturnService  -..->  IBusinessProductReturnRepository 
BusinessProductReturnService  --*  IBusinessProductReturnRepository 
BusinessProductReturnService  -->  IBusinessProductReturnRepository 
BusinessProductReturnService  -..-|>  IBusinessProductReturnService 
BusinessProductService  -->  IBusinessProductRepository 
BusinessProductService  -..->  IBusinessProductRepository 
BusinessProductService  --*  IBusinessProductRepository 
BusinessProductService  -..-|>  IBusinessProductService 
BusinessProductService  -->  ProductCreateDto 
BusinessWarrantyClaimController  -->  BusinessInvalidStatusChangeException 
BusinessWarrantyClaimController  -->  BusinessOwnershipException 
BusinessWarrantyClaimController  -->  BusinessWarrantyClaimDto 
BusinessWarrantyClaimController  --*  IBusinessWarrantyClaimService 
BusinessWarrantyClaimController  -->  IBusinessWarrantyClaimService 
BusinessWarrantyClaimController  -..->  IBusinessWarrantyClaimService 
BusinessWarrantyClaimController  -->  UpdateBusinessWarrantyClaimStatusDto 
BusinessWarrantyClaimRepository  -->  BusinessInvalidStatusChangeException 
BusinessWarrantyClaimRepository  -->  BusinessOwnershipException 
BusinessWarrantyClaimRepository  -->  BusinessWarrantyClaimDto 
BusinessWarrantyClaimRepository  -..-|>  IBusinessWarrantyClaimRepository 
BusinessWarrantyClaimService  -->  BusinessWarrantyClaimDto 
BusinessWarrantyClaimService  -..->  IBusinessWarrantyClaimRepository 
BusinessWarrantyClaimService  -->  IBusinessWarrantyClaimRepository 
BusinessWarrantyClaimService  --*  IBusinessWarrantyClaimRepository 
BusinessWarrantyClaimService  -..-|>  IBusinessWarrantyClaimService 
BuyerProductService  -..-|>  IBuyerProductService 
BuyerProductService  -->  IProductRepository 
BuyerProductService  -..->  IProductRepository 
BuyerProductService  --*  IProductRepository 
BuyerProductsController  -..->  IBuyerProductService 
BuyerProductsController  --*  IBuyerProductService 
BuyerProductsController  -->  IBuyerProductService 
CategoryController  -->  ICategoryService 
CategoryController  --*  ICategoryService 
CategoryController  -..->  ICategoryService 
CategoryRepository  -..-|>  ICategoryRepository 
CategoryService  -..->  ICategoryRepository 
CategoryService  --*  ICategoryRepository 
CategoryService  -->  ICategoryRepository 
CategoryService  -..-|>  ICategoryService 
FavouriteController  -->  IFavouriteService 
FavouriteController  -..->  IFavouriteService 
FavouriteController  --*  IFavouriteService 
FavouriteRepository  -..-|>  IFavouriteRepository 
FavouriteRepository  --*  IProductRepository 
FavouriteRepository  -..->  IProductRepository 
FavouriteService  -..->  IFavouriteRepository 
FavouriteService  -->  IFavouriteRepository 
FavouriteService  --*  IFavouriteRepository 
FavouriteService  -..-|>  IFavouriteService 
GudenaPaymentClient  -->  CreditCardDto 
GudenaShippingClient  -->  LocationsShippingsDto 
IBusinessOrderRepository  -->  BusinessOrderDto 
IBusinessOrderService  -->  BusinessOrderDto 
IBusinessProductReturnRepository  -->  BusinessProductReturnDto 
IBusinessProductReturnService  -->  BusinessProductReturnDto 
IBusinessProductService  -->  ProductCreateDto 
IBusinessWarrantyClaimRepository  -->  BusinessWarrantyClaimDto 
IBusinessWarrantyClaimService  -->  BusinessWarrantyClaimDto 
IOrderRepository  -->  OrderUpdateDto 
IOrderService  -->  OrderUpdateDto 
IProductReturnRepository  -->  ProductReturnDto 
IProductReturnService  -->  ProductReturnDto 
IWarrantyClaimRepository  -->  WarrantyClaimDto 
IWarrantyClaimService  -->  WarrantyClaimDto 
OrderController  -->  CannotModifyOrderException 
OrderController  -->  IOrderService 
OrderController  -..->  IOrderService 
OrderController  --*  IOrderService 
OrderController  -->  IPaymentService 
OrderController  --*  IPaymentService 
OrderController  -..->  IPaymentService 
OrderController  -->  IncancellableOrderException 
OrderController  -->  OrderExceedsStockException 
OrderController  -->  OrderUpdateDto 
OrderController  -->  ResourceNotFoundException 
OrderController  -->  UnpaidException 
OrderController  -->  UserDoesNotOwnResourceException 
OrderItemRepository  -..-|>  IOrderItemRepository 
OrderItemService  --*  IOrderItemRepository 
OrderItemService  -->  IOrderItemRepository 
OrderItemService  -..->  IOrderItemRepository 
OrderItemService  -..-|>  IOrderItemService 
OrderRepository  -->  CannotModifyOrderException 
OrderRepository  --*  IBasketRepository 
OrderRepository  -..->  IBasketRepository 
OrderRepository  -->  IBasketRepository 
OrderRepository  -..-|>  IOrderRepository 
OrderRepository  -..->  IPaymentRepository 
OrderRepository  --*  IPaymentRepository 
OrderRepository  -->  IPaymentRepository 
OrderRepository  -..->  IProductRepository 
OrderRepository  --*  IProductRepository 
OrderRepository  -->  IProductRepository 
OrderRepository  -->  IShippingRepository 
OrderRepository  --*  IShippingRepository 
OrderRepository  -..->  IShippingRepository 
OrderRepository  -->  IncancellableOrderException 
OrderRepository  -->  OrderExceedsStockException 
OrderRepository  -->  OrderItemDto 
OrderRepository  -->  OrderUpdateDto 
OrderRepository  -->  ResourceNotFoundException 
OrderRepository  -->  UnpaidException 
OrderRepository  -->  UserDoesNotOwnResourceException 
OrderService  --*  IOrderRepository 
OrderService  -..->  IOrderRepository 
OrderService  -->  IOrderRepository 
OrderService  -..-|>  IOrderService 
OrderService  -->  OrderUpdateDto 
OrderUpdateDto  -->  OrderItemDto 
PaymentController  -->  GudenaPaymentClient 
PaymentController  --*  IBasketService 
PaymentController  -->  IBasketService 
PaymentController  -..->  IBasketService 
PaymentController  --*  IPaymentService 
PaymentController  -->  IPaymentService 
PaymentController  -..->  IPaymentService 
PaymentController  -->  IShippingService 
PaymentController  --*  IShippingService 
PaymentController  -..->  IShippingService 
PaymentController  -->  PaymentDto 
PaymentDto  -->  CreditCardDto 
PaymentDto  -->  CreditCardDto 
PaymentRepository  -..-|>  IPaymentRepository 
PaymentService  -..->  IPaymentRepository 
PaymentService  --*  IPaymentRepository 
PaymentService  -->  IPaymentRepository 
PaymentService  -..-|>  IPaymentService 
ProductRepository  -..-|>  IProductRepository 
ProductReturnController  -..->  IProductReturnService 
ProductReturnController  --*  IProductReturnService 
ProductReturnController  -->  IProductReturnService 
ProductReturnController  -->  NonReturnableProductException 
ProductReturnController  -->  ProductAlreadyReturnedException 
ProductReturnController  -->  ProductReturnDto 
ProductReturnController  -->  ResourceNotFoundException 
ProductReturnController  -->  UserDoesNotOwnResourceException 
ProductReturnRepository  -..-|>  IProductReturnRepository 
ProductReturnRepository  -->  NonReturnableProductException 
ProductReturnRepository  -->  ProductAlreadyReturnedException 
ProductReturnRepository  -->  ProductReturnDto 
ProductReturnRepository  -->  ResourceNotFoundException 
ProductReturnRepository  -->  UserDoesNotOwnResourceException 
ProductReturnService  -..->  IProductReturnRepository 
ProductReturnService  --*  IProductReturnRepository 
ProductReturnService  -->  IProductReturnRepository 
ProductReturnService  -..-|>  IProductReturnService 
ProductReturnService  -->  ProductReturnDto 
ShippingController  -->  CreateShippingDto 
ShippingController  -->  GudenaShippingClient 
ShippingController  -..->  IAccountDetailsService 
ShippingController  -->  IAccountDetailsService 
ShippingController  --*  IAccountDetailsService 
ShippingController  -->  IBasketService 
ShippingController  --*  IBasketService 
ShippingController  -..->  IBasketService 
ShippingController  -..->  IOrderItemService 
ShippingController  --*  IOrderItemService 
ShippingController  -->  IShippingService 
ShippingController  --*  IShippingService 
ShippingController  -..->  IShippingService 
ShippingController  -->  UpdateShippingDto 
ShippingRepository  -..-|>  IShippingRepository 
ShippingService  -..->  IShippingRepository 
ShippingService  -->  IShippingRepository 
ShippingService  --*  IShippingRepository 
ShippingService  -..-|>  IShippingService 
WarrantyClaimController  -..->  IWarrantyClaimService 
WarrantyClaimController  -->  IWarrantyClaimService 
WarrantyClaimController  --*  IWarrantyClaimService 
WarrantyClaimController  -->  ResourceNotFoundException 
WarrantyClaimController  -->  UserDoesNotOwnResourceException 
WarrantyClaimController  -->  WarrantyAlreadyClaimedException 
WarrantyClaimController  -->  WarrantyClaimDto 
WarrantyClaimRepository  -..-|>  IWarrantyClaimRepository 
WarrantyClaimRepository  -->  ResourceNotFoundException 
WarrantyClaimRepository  -->  UserDoesNotOwnResourceException 
WarrantyClaimRepository  -->  WarrantyAlreadyClaimedException 
WarrantyClaimRepository  -->  WarrantyClaimDto 
WarrantyClaimService  -->  IWarrantyClaimRepository 
WarrantyClaimService  -..->  IWarrantyClaimRepository 
WarrantyClaimService  --*  IWarrantyClaimRepository 
WarrantyClaimService  -..-|>  IWarrantyClaimService 
WarrantyClaimService  -->  WarrantyClaimDto 
