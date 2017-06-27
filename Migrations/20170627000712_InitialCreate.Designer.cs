using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using monolithic_shop_core.Data;

namespace monolithicshopcore.Migrations
{
    [DbContext(typeof(MainDatabaseContext))]
    [Migration("20170627000712_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2");

            modelBuilder.Entity("monolithic_shop_core.Data.Address", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("City");

                    b.Property<string>("Street");

                    b.HasKey("Id");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("monolithic_shop_core.Data.Buyer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Buyers");
                });

            modelBuilder.Entity("monolithic_shop_core.Data.Card", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Number");

                    b.HasKey("Id");

                    b.ToTable("Card");
                });

            modelBuilder.Entity("monolithic_shop_core.Data.Order", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("BuyerId");

                    b.Property<int>("Status");

                    b.HasKey("Id");

                    b.HasIndex("BuyerId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("monolithic_shop_core.Data.Payment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("AddressId");

                    b.Property<int?>("CardId");

                    b.Property<Guid>("OrderId");

                    b.Property<decimal>("Price");

                    b.Property<int>("Status");

                    b.Property<int>("Type");

                    b.HasKey("Id");

                    b.HasIndex("AddressId");

                    b.HasIndex("CardId");

                    b.HasIndex("OrderId")
                        .IsUnique();

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("monolithic_shop_core.Data.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<decimal>("Price");

                    b.HasKey("Id");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("monolithic_shop_core.Data.ProductOrder", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Count");

                    b.Property<Guid?>("OrderId");

                    b.Property<int>("ProductId");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductOrders");
                });

            modelBuilder.Entity("monolithic_shop_core.Data.ProductWarehouse", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("NumberAvailable");

                    b.Property<int?>("ProductId");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductsWarehouse");
                });

            modelBuilder.Entity("monolithic_shop_core.Data.Order", b =>
                {
                    b.HasOne("monolithic_shop_core.Data.Buyer", "Buyer")
                        .WithMany()
                        .HasForeignKey("BuyerId");
                });

            modelBuilder.Entity("monolithic_shop_core.Data.Payment", b =>
                {
                    b.HasOne("monolithic_shop_core.Data.Address", "Address")
                        .WithMany()
                        .HasForeignKey("AddressId");

                    b.HasOne("monolithic_shop_core.Data.Card", "Card")
                        .WithMany()
                        .HasForeignKey("CardId");

                    b.HasOne("monolithic_shop_core.Data.Order")
                        .WithOne("Payment")
                        .HasForeignKey("monolithic_shop_core.Data.Payment", "OrderId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("monolithic_shop_core.Data.ProductOrder", b =>
                {
                    b.HasOne("monolithic_shop_core.Data.Order")
                        .WithMany("ProductOrders")
                        .HasForeignKey("OrderId");

                    b.HasOne("monolithic_shop_core.Data.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("monolithic_shop_core.Data.ProductWarehouse", b =>
                {
                    b.HasOne("monolithic_shop_core.Data.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId");
                });
        }
    }
}
