import { useEffect, useState } from "react";
import api from "../api";
import { type Product } from "../types/Product";
import ProductCard from "../components/ProductCard";
import { mockProducts } from "../temp/mockProducts";

export default function Home() {
  const [products, setProducts] = useState<Product[]>([]);

  // useEffect(() => {
  //   api
  //     .get("/products")
  //     .then((res) => {
  //       const data = res.data;
  //       setProducts(Array.isArray(data) ? data : (data.products ?? []));
  //     })
  //     .catch(console.error);
  // }, []);

  useEffect(() => {
    setProducts(mockProducts);
  }, []);

  return (
    <div className="min-h-screen">
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
