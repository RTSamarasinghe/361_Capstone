import { useEffect, useState } from "react";
import api from "../api";
import { type Product } from "../types/Product";
import ProductCard from "../components/ProductCard";
import { mockProducts } from "../temp/mockProducts";

export default function Home() {
  const [products, setProducts] = useState<Product[]>([]);
  const [showSuccess, setShowSuccess] = useState(false);

  useEffect(() => {
    api
      .get<Product[]>("/products")
      .then((res) => {
        console.log(res.data);
        setProducts(res.data);
      })
      .catch((err) => {
        console.error("Failed to fetch products:", err);
      });
  }, []);

  // useEffect(() => {
  //   // Display Order Placed
  //   if (localStorage.getItem("orderSuccess")) {
  //     setShowSuccess(true);
  //     localStorage.removeItem("orderSuccess");

  //     setTimeout(() => setShowSuccess(false), 3000);
  //   }
  //   // Add Products
  //   setProducts(mockProducts);
  // }, []);

  return (
    <div className="min-h-screen">
      {showSuccess && (
        <div className="mb-6 p-4 bg-green-100 text-green-700 rounded-md">
          Order placed successfully!
        </div>
      )}
      {/* Page Header */}
      <div className="mb-10">
        <h1 className="text-4xl font-bold text-gray-800 mb-2">
          Featured Products
        </h1>
        <p className="text-gray-600">Browse our latest collection.</p>
      </div>

      {/* Product Grid */}
      <div className="grid gap-8 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4">
        {products.map((product) => (
          <ProductCard key={product.id} product={product} />
        ))}
      </div>
    </div>
  );
}
