/SQL Schema + Data


USE [EcommerceClothShop]
GO
/****** Object:  Table [dbo].[Cart]    Script Date: 5/16/2025 12:57:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cart](
	[CartID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NULL,
	[ProductID] [int] NULL,
	[Quantity] [int] NOT NULL,
	[AddedAt] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[CartID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Categories]    Script Date: 5/16/2025 12:57:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categories](
	[CategoryID] [int] IDENTITY(1,1) NOT NULL,
	[CategoryName] [nvarchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[CategoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DiscountPromos]    Script Date: 5/16/2025 12:57:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DiscountPromos](
	[PromoID] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](50) NOT NULL,
	[DiscountAmount] [decimal](10, 2) NOT NULL,
	[IsPercentage] [bit] NOT NULL,
	[MinOrderAmount] [decimal](10, 2) NULL,
	[ExpiryDate] [date] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[PromoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderDetails]    Script Date: 5/16/2025 12:57:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderDetails](
	[OrderDetailID] [int] IDENTITY(1,1) NOT NULL,
	[OrderID] [int] NULL,
	[ProductID] [int] NULL,
	[Quantity] [int] NOT NULL,
	[Price] [decimal](10, 2) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[OrderDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderPromos]    Script Date: 5/16/2025 12:57:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderPromos](
	[OrderPromoID] [int] IDENTITY(1,1) NOT NULL,
	[OrderID] [int] NULL,
	[PromoID] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[OrderPromoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Orders]    Script Date: 5/16/2025 12:57:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Orders](
	[OrderID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NULL,
	[TotalAmount] [decimal](10, 2) NOT NULL,
	[OrderStatus] [nvarchar](50) NOT NULL,
	[CreatedAt] [datetime] NULL,
	[PaymentMethod] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[OrderID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Payments]    Script Date: 5/16/2025 12:57:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Payments](
	[PaymentID] [int] IDENTITY(1,1) NOT NULL,
	[OrderID] [int] NULL,
	[PaymentMethod] [nvarchar](50) NULL,
	[PaymentStatus] [nvarchar](50) NULL,
	[PaidAt] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[PaymentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductDiscounts]    Script Date: 5/16/2025 12:57:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductDiscounts](
	[ProductDiscountID] [int] IDENTITY(1,1) NOT NULL,
	[ProductID] [int] NULL,
	[DiscountAmount] [decimal](10, 2) NOT NULL,
	[IsPercentage] [bit] NOT NULL,
	[StartDate] [date] NOT NULL,
	[EndDate] [date] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ProductDiscountID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Products]    Script Date: 5/16/2025 12:57:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Products](
	[ProductID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Price] [decimal](10, 2) NOT NULL,
	[Stock] [int] NOT NULL,
	[CategoryID] [int] NULL,
	[ImageURL] [nvarchar](255) NULL,
	[CreatedAt] [datetime] NULL,
	[IsDeleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[ProductID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Reviews]    Script Date: 5/16/2025 12:57:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Reviews](
	[ReviewID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NULL,
	[ProductID] [int] NULL,
	[Rating] [int] NULL,
	[Comment] [nvarchar](max) NULL,
	[CreatedAt] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[ReviewID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 5/16/2025 12:57:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[FullName] [nvarchar](100) NOT NULL,
	[Email] [nvarchar](100) NOT NULL,
	[PasswordHash] [nvarchar](255) NOT NULL,
	[Phone] [nvarchar](15) NULL,
	[Address] [nvarchar](255) NULL,
	[Role] [nvarchar](50) NOT NULL,
	[CreatedAt] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Categories] ON 
GO
INSERT [dbo].[Categories] ([CategoryID], [CategoryName]) VALUES (2, N'Hoodie')
GO
INSERT [dbo].[Categories] ([CategoryID], [CategoryName]) VALUES (1, N'T-Shirt')
GO
SET IDENTITY_INSERT [dbo].[Categories] OFF
GO
SET IDENTITY_INSERT [dbo].[OrderDetails] ON 
GO
INSERT [dbo].[OrderDetails] ([OrderDetailID], [OrderID], [ProductID], [Quantity], [Price]) VALUES (2, 2, 23, 2, CAST(200000.00 AS Decimal(10, 2)))
GO
INSERT [dbo].[OrderDetails] ([OrderDetailID], [OrderID], [ProductID], [Quantity], [Price]) VALUES (3, 3, 23, 1, CAST(200000.00 AS Decimal(10, 2)))
GO
INSERT [dbo].[OrderDetails] ([OrderDetailID], [OrderID], [ProductID], [Quantity], [Price]) VALUES (4, 4, 23, 2, CAST(200000.00 AS Decimal(10, 2)))
GO
INSERT [dbo].[OrderDetails] ([OrderDetailID], [OrderID], [ProductID], [Quantity], [Price]) VALUES (1003, 1003, 24, 1, CAST(200000.00 AS Decimal(10, 2)))
GO
SET IDENTITY_INSERT [dbo].[OrderDetails] OFF
GO
SET IDENTITY_INSERT [dbo].[Orders] ON 
GO
INSERT [dbo].[Orders] ([OrderID], [UserID], [TotalAmount], [OrderStatus], [CreatedAt], [PaymentMethod]) VALUES (1, 3, CAST(200000.00 AS Decimal(10, 2)), N'Pending', CAST(N'2025-03-17T14:19:36.233' AS DateTime), N'vnpay')
GO
INSERT [dbo].[Orders] ([OrderID], [UserID], [TotalAmount], [OrderStatus], [CreatedAt], [PaymentMethod]) VALUES (2, 3, CAST(400000.00 AS Decimal(10, 2)), N'Pending', CAST(N'2025-03-17T22:58:15.013' AS DateTime), N'vnpay')
GO
INSERT [dbo].[Orders] ([OrderID], [UserID], [TotalAmount], [OrderStatus], [CreatedAt], [PaymentMethod]) VALUES (3, 3, CAST(200000.00 AS Decimal(10, 2)), N'Pending', CAST(N'2025-03-18T13:02:54.183' AS DateTime), N'vnpay')
GO
INSERT [dbo].[Orders] ([OrderID], [UserID], [TotalAmount], [OrderStatus], [CreatedAt], [PaymentMethod]) VALUES (4, 3, CAST(400000.00 AS Decimal(10, 2)), N'Pending', CAST(N'2025-03-18T13:59:53.333' AS DateTime), N'vnpay')
GO
INSERT [dbo].[Orders] ([OrderID], [UserID], [TotalAmount], [OrderStatus], [CreatedAt], [PaymentMethod]) VALUES (1003, 3, CAST(200000.00 AS Decimal(10, 2)), N'Pending', CAST(N'2025-03-30T00:26:39.063' AS DateTime), N'vnpay')
GO
SET IDENTITY_INSERT [dbo].[Orders] OFF
GO
SET IDENTITY_INSERT [dbo].[ProductDiscounts] ON 
GO
INSERT [dbo].[ProductDiscounts] ([ProductDiscountID], [ProductID], [DiscountAmount], [IsPercentage], [StartDate], [EndDate]) VALUES (5, 23, CAST(50000.00 AS Decimal(10, 2)), 0, CAST(N'2025-03-17' AS Date), CAST(N'2025-03-27' AS Date))
GO
INSERT [dbo].[ProductDiscounts] ([ProductDiscountID], [ProductID], [DiscountAmount], [IsPercentage], [StartDate], [EndDate]) VALUES (6, 24, CAST(10.00 AS Decimal(10, 2)), 1, CAST(N'2025-03-17' AS Date), CAST(N'2025-04-01' AS Date))
GO
INSERT [dbo].[ProductDiscounts] ([ProductDiscountID], [ProductID], [DiscountAmount], [IsPercentage], [StartDate], [EndDate]) VALUES (7, 30, CAST(20.00 AS Decimal(10, 2)), 1, CAST(N'2025-03-17' AS Date), CAST(N'2025-04-06' AS Date))
GO
INSERT [dbo].[ProductDiscounts] ([ProductDiscountID], [ProductID], [DiscountAmount], [IsPercentage], [StartDate], [EndDate]) VALUES (8, 34, CAST(100000.00 AS Decimal(10, 2)), 0, CAST(N'2025-03-17' AS Date), CAST(N'2025-03-24' AS Date))
GO
SET IDENTITY_INSERT [dbo].[ProductDiscounts] OFF
GO
SET IDENTITY_INSERT [dbo].[Products] ON 
GO
INSERT [dbo].[Products] ([ProductID], [Name], [Description], [Price], [Stock], [CategoryID], [ImageURL], [CreatedAt], [IsDeleted]) VALUES (23, N'CHUNITHM Elisha Murcia T-shirt', N'Size: L', CAST(200000.00 AS Decimal(10, 2)), 70, 1, N'/Content/ProductImages/CHUNITHM Elisha Murcia T-shirt.jpg', CAST(N'2025-03-17T14:26:39.797' AS DateTime), 0)
GO
INSERT [dbo].[Products] ([ProductID], [Name], [Description], [Price], [Stock], [CategoryID], [ImageURL], [CreatedAt], [IsDeleted]) VALUES (24, N'CHUNITHM Misra Tersera T Shirt', N'Size: XL', CAST(200000.00 AS Decimal(10, 2)), 79, 1, N'/Content/ProductImages/CHUNITHM Misra Tersera T Shirt.jpg', CAST(N'2025-03-17T14:26:39.797' AS DateTime), 0)
GO
INSERT [dbo].[Products] ([ProductID], [Name], [Description], [Price], [Stock], [CategoryID], [ImageURL], [CreatedAt], [IsDeleted]) VALUES (25, N'CHUNITHM Men''s Nye T-Shirt', N'Size: M', CAST(200000.00 AS Decimal(10, 2)), 50, 1, N'/Content/ProductImages/CHUNITHM Men''s Nye T-Shirt.jpg', CAST(N'2025-03-17T14:26:39.797' AS DateTime), 0)
GO
INSERT [dbo].[Products] ([ProductID], [Name], [Description], [Price], [Stock], [CategoryID], [ImageURL], [CreatedAt], [IsDeleted]) VALUES (26, N'CHUNITHM Eva Dominance XII T Shirt', N'Size: L', CAST(200000.00 AS Decimal(10, 2)), 60, 1, N'/Content/ProductImages/CHUNITHM Eva Dominance XII T Shirt.jpg', CAST(N'2025-03-17T14:26:39.797' AS DateTime), 0)
GO
INSERT [dbo].[Products] ([ProductID], [Name], [Description], [Price], [Stock], [CategoryID], [ImageURL], [CreatedAt], [IsDeleted]) VALUES (27, N'CHUNITHM Eva Dominance XII Hoodie', N'Size: XL', CAST(400000.00 AS Decimal(10, 2)), 55, 2, N'/Content/ProductImages/CHUNITHM Eva Dominance XII Hoodie.jpg', CAST(N'2025-03-17T14:26:39.797' AS DateTime), 0)
GO
INSERT [dbo].[Products] ([ProductID], [Name], [Description], [Price], [Stock], [CategoryID], [ImageURL], [CreatedAt], [IsDeleted]) VALUES (28, N'CHUNITHM Kainan Melvias Back Print Hoodie', N'Size: XXL', CAST(400000.00 AS Decimal(10, 2)), 70, 2, N'/Content/ProductImages/CHUNITHM Kainan Melvias Back Print Hoodie.jpg', CAST(N'2025-03-17T14:26:39.797' AS DateTime), 0)
GO
INSERT [dbo].[Products] ([ProductID], [Name], [Description], [Price], [Stock], [CategoryID], [ImageURL], [CreatedAt], [IsDeleted]) VALUES (29, N'CHUNITHM Toa Women''s T-Shirt', N'Size: S', CAST(200000.00 AS Decimal(10, 2)), 65, 1, N'/Content/ProductImages/CHUNITHM Toa Women''s T-Shirt.jpg', CAST(N'2025-03-17T14:26:39.797' AS DateTime), 0)
GO
INSERT [dbo].[Products] ([ProductID], [Name], [Description], [Price], [Stock], [CategoryID], [ImageURL], [CreatedAt], [IsDeleted]) VALUES (30, N'CHUNITHM Lena Ishmeil T-Shirt', N'Size: M', CAST(200000.00 AS Decimal(10, 2)), 75, 1, N'/Content/ProductImages/CHUNITHM Lena Ishmeil T-Shirt.jpg', CAST(N'2025-03-17T14:26:39.797' AS DateTime), 0)
GO
INSERT [dbo].[Products] ([ProductID], [Name], [Description], [Price], [Stock], [CategoryID], [ImageURL], [CreatedAt], [IsDeleted]) VALUES (31, N'CHUNITHM Mene Tercera T-Shirt', N'Size: XL', CAST(200000.00 AS Decimal(10, 2)), 85, 1, N'/Content/ProductImages/CHUNITHM Mene Tercera T-Shirt.jpg', CAST(N'2025-03-17T14:26:39.797' AS DateTime), 0)
GO
INSERT [dbo].[Products] ([ProductID], [Name], [Description], [Price], [Stock], [CategoryID], [ImageURL], [CreatedAt], [IsDeleted]) VALUES (32, N'maimai Rainbow Rush Story T-Shirt', N'Size: L', CAST(200000.00 AS Decimal(10, 2)), 90, 1, N'/Content/ProductImages/maimai Rainbow Rush Story T-Shirt.jpg', CAST(N'2025-03-17T14:26:39.797' AS DateTime), 0)
GO
INSERT [dbo].[Products] ([ProductID], [Name], [Description], [Price], [Stock], [CategoryID], [ImageURL], [CreatedAt], [IsDeleted]) VALUES (33, N'maimai Delax Shama & Miruku Yunibar Version T-Shirt', N'Size: S', CAST(200000.00 AS Decimal(10, 2)), 80, 1, N'/Content/ProductImages/maimai Delax Shama & Miruku Yunibar Version T-Shirt.jpg', CAST(N'2025-03-17T14:26:39.797' AS DateTime), 0)
GO
INSERT [dbo].[Products] ([ProductID], [Name], [Description], [Price], [Stock], [CategoryID], [ImageURL], [CreatedAt], [IsDeleted]) VALUES (34, N'maimai Rainbow Rush Story Hoodie', N'Size: XL', CAST(400000.00 AS Decimal(10, 2)), 60, 2, N'/Content/ProductImages/maimai Rainbow Rush Story Hoodie.jpg', CAST(N'2025-03-17T14:26:39.797' AS DateTime), 0)
GO
INSERT [dbo].[Products] ([ProductID], [Name], [Description], [Price], [Stock], [CategoryID], [ImageURL], [CreatedAt], [IsDeleted]) VALUES (35, N'maimai Delax Group T-Shirt', N'Size: M', CAST(200000.00 AS Decimal(10, 2)), 50, 1, N'/Content/ProductImages/maimai Delax Group T-Shirt.jpg', CAST(N'2025-03-17T14:26:39.797' AS DateTime), 0)
GO
INSERT [dbo].[Products] ([ProductID], [Name], [Description], [Price], [Stock], [CategoryID], [ImageURL], [CreatedAt], [IsDeleted]) VALUES (36, N'Ongeki Bright MEMORY Tsuna T-shirt Vol.2', N'Size: L', CAST(200000.00 AS Decimal(10, 2)), 55, 1, N'/Content/ProductImages/Ongeki Bright MEMORY Tsuna T-shirt Vol.2.jpg', CAST(N'2025-03-17T14:26:39.797' AS DateTime), 0)
GO
INSERT [dbo].[Products] ([ProductID], [Name], [Description], [Price], [Stock], [CategoryID], [ImageURL], [CreatedAt], [IsDeleted]) VALUES (37, N'Ongeki Bright MEMORY R.B.P. T-Shirt', N'Size: XL', CAST(200000.00 AS Decimal(10, 2)), 65, 1, N'/Content/ProductImages/Ongeki Bright MEMORY R.B.P. T-Shirt.jpg', CAST(N'2025-03-17T14:26:39.797' AS DateTime), 0)
GO
INSERT [dbo].[Products] ([ProductID], [Name], [Description], [Price], [Stock], [CategoryID], [ImageURL], [CreatedAt], [IsDeleted]) VALUES (38, N'Ongeki Bright MEMORY R.B.P. Back Print Hoodie', N'Size: XXL', CAST(400000.00 AS Decimal(10, 2)), 75, 2, N'/Content/ProductImages/Ongeki Bright MEMORY R.B.P. Back Print Hoodie.jpg', CAST(N'2025-03-17T14:26:39.797' AS DateTime), 0)
GO
INSERT [dbo].[Products] ([ProductID], [Name], [Description], [Price], [Stock], [CategoryID], [ImageURL], [CreatedAt], [IsDeleted]) VALUES (39, N'Ongeki Bright MEMORY ASTERISM T-Shirt', N'Size: M', CAST(200000.00 AS Decimal(10, 2)), 85, 1, N'/Content/ProductImages/Ongeki Bright MEMORY ASTERISM T-Shirt.jpg', CAST(N'2025-03-17T14:26:39.797' AS DateTime), 0)
GO
INSERT [dbo].[Products] ([ProductID], [Name], [Description], [Price], [Stock], [CategoryID], [ImageURL], [CreatedAt], [IsDeleted]) VALUES (40, N'Ongeki Bright MEMORY ASTERISM Back Print Hoodie', N'Size: XL', CAST(400000.00 AS Decimal(10, 2)), 50, 2, N'/Content/ProductImages/Ongeki Bright MEMORY ASTERISM Back Print Hoodie.jpg', CAST(N'2025-03-17T14:26:39.797' AS DateTime), 0)
GO
INSERT [dbo].[Products] ([ProductID], [Name], [Description], [Price], [Stock], [CategoryID], [ImageURL], [CreatedAt], [IsDeleted]) VALUES (41, N'Ongeki Bright MEMORY Setuna T-Shirt', N'Size: L', CAST(200000.00 AS Decimal(10, 2)), 95, 1, N'/Content/ProductImages/Ongeki Bright MEMORY Setuna T-Shirt.jpg', CAST(N'2025-03-17T14:26:39.797' AS DateTime), 0)
GO
INSERT [dbo].[Products] ([ProductID], [Name], [Description], [Price], [Stock], [CategoryID], [ImageURL], [CreatedAt], [IsDeleted]) VALUES (42, N'Ongeki Bright MEMORY TRiEDGE T-Shirt', N'Size: S', CAST(200000.00 AS Decimal(10, 2)), 100, 1, N'/Content/ProductImages/Ongeki Bright MEMORY TRiEDGE T-Shirt.jpg', CAST(N'2025-03-17T14:26:39.797' AS DateTime), 0)
GO
INSERT [dbo].[Products] ([ProductID], [Name], [Description], [Price], [Stock], [CategoryID], [ImageURL], [CreatedAt], [IsDeleted]) VALUES (43, N'SOUND VOLTEX EXCEED GEAR Lacis & Grace T-Shirt', N'Size: M', CAST(200000.00 AS Decimal(10, 2)), 60, 1, N'/Content/ProductImages/SOUND VOLTEX EXCEED GEAR Lacis & Grace T-Shirt.jpg', CAST(N'2025-03-17T14:26:39.797' AS DateTime), 0)
GO
SET IDENTITY_INSERT [dbo].[Products] OFF
GO
SET IDENTITY_INSERT [dbo].[Reviews] ON 
GO
INSERT [dbo].[Reviews] ([ReviewID], [UserID], [ProductID], [Rating], [Comment], [CreatedAt]) VALUES (1, 3, 23, 5, N'a', CAST(N'2025-03-24T12:28:28.490' AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[Reviews] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON 
GO
INSERT [dbo].[Users] ([UserID], [FullName], [Email], [PasswordHash], [Phone], [Address], [Role], [CreatedAt]) VALUES (3, N'Lotus', N'abc@mail.com', N'12345678', N'0123456789', N'abc', N'admin', CAST(N'2025-03-17T14:18:13.090' AS DateTime))
GO
INSERT [dbo].[Users] ([UserID], [FullName], [Email], [PasswordHash], [Phone], [Address], [Role], [CreatedAt]) VALUES (4, N'Vĩnh Huy Thân', N'k4sp3rofficial@gmail.com', N'18042004', N'0907803197', N'asd', N'customer', CAST(N'2025-03-24T12:26:50.850' AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Categori__8517B2E01C0556CE]    Script Date: 5/16/2025 12:57:55 PM ******/
ALTER TABLE [dbo].[Categories] ADD UNIQUE NONCLUSTERED 
(
	[CategoryName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Discount__A25C5AA7DF521381]    Script Date: 5/16/2025 12:57:55 PM ******/
ALTER TABLE [dbo].[DiscountPromos] ADD UNIQUE NONCLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Users__A9D1053467379342]    Script Date: 5/16/2025 12:57:55 PM ******/
ALTER TABLE [dbo].[Users] ADD UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Cart] ADD  DEFAULT (getdate()) FOR [AddedAt]
GO
ALTER TABLE [dbo].[Orders] ADD  DEFAULT ('pending') FOR [OrderStatus]
GO
ALTER TABLE [dbo].[Orders] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Products] ADD  DEFAULT ((0)) FOR [Stock]
GO
ALTER TABLE [dbo].[Products] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Products] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Reviews] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT ('customer') FOR [Role]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Cart]  WITH CHECK ADD FOREIGN KEY([ProductID])
REFERENCES [dbo].[Products] ([ProductID])
GO
ALTER TABLE [dbo].[Cart]  WITH CHECK ADD FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[OrderDetails]  WITH CHECK ADD FOREIGN KEY([OrderID])
REFERENCES [dbo].[Orders] ([OrderID])
GO
ALTER TABLE [dbo].[OrderDetails]  WITH CHECK ADD FOREIGN KEY([ProductID])
REFERENCES [dbo].[Products] ([ProductID])
GO
ALTER TABLE [dbo].[OrderPromos]  WITH CHECK ADD FOREIGN KEY([OrderID])
REFERENCES [dbo].[Orders] ([OrderID])
GO
ALTER TABLE [dbo].[OrderPromos]  WITH CHECK ADD FOREIGN KEY([PromoID])
REFERENCES [dbo].[DiscountPromos] ([PromoID])
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Payments]  WITH CHECK ADD FOREIGN KEY([OrderID])
REFERENCES [dbo].[Orders] ([OrderID])
GO
ALTER TABLE [dbo].[ProductDiscounts]  WITH CHECK ADD FOREIGN KEY([ProductID])
REFERENCES [dbo].[Products] ([ProductID])
GO
ALTER TABLE [dbo].[Products]  WITH CHECK ADD FOREIGN KEY([CategoryID])
REFERENCES [dbo].[Categories] ([CategoryID])
GO
ALTER TABLE [dbo].[Reviews]  WITH CHECK ADD FOREIGN KEY([ProductID])
REFERENCES [dbo].[Products] ([ProductID])
GO
ALTER TABLE [dbo].[Reviews]  WITH CHECK ADD FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Cart]  WITH CHECK ADD CHECK  (([Quantity]>(0)))
GO
ALTER TABLE [dbo].[OrderDetails]  WITH CHECK ADD CHECK  (([Quantity]>(0)))
GO
ALTER TABLE [dbo].[Payments]  WITH CHECK ADD CHECK  (([PaymentMethod]='credit_card' OR [PaymentMethod]='paypal' OR [PaymentMethod]='bank_transfer' OR [PaymentMethod]='cod'))
GO
ALTER TABLE [dbo].[Payments]  WITH CHECK ADD CHECK  (([PaymentStatus]='failed' OR [PaymentStatus]='completed' OR [PaymentStatus]='pending'))
GO
ALTER TABLE [dbo].[Reviews]  WITH CHECK ADD CHECK  (([Rating]>=(1) AND [Rating]<=(5)))
GO
