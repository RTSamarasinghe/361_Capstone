import { useEffect, useState } from "react";
import api from "../api.ts";
import { type Product } from "../types/Product.ts";
import { useNavigate } from "react-router-dom";

export default function Home() {
  const [products, setProducts] = useState<Product[]>([]);
  const navigate = useNavigate();

  useEffect(() => {
    api
      .get("/products")
      .then((res) => {
        const data = res.data;
        setProducts(Array.isArray(data) ? data : (data.products ?? []));
      })
      .catch(console.error);
  }, []);

  return (
    <div className="min-h-screen bg-gray-50 p-8">
      <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-6">
        {products.map((product) => (
          <div
            key={product.id}
            className="bg-white rounded-2xl shadow-md overflow-hidden hover:shadow-lg transition"
          >
            <img
              src={product.imageUrl}
              alt={product.name}
              className="h-48 w-full object-cover"
            />

            <div className="p-4">
              <h2 className="text-lg font-semibold">{product.name}</h2>

              <p className="text-gray-600 mt-1">${product.price.toFixed(2)}</p>

              <button className="mt-4 w-full bg-black text-white py-2 rounded-xl hover:bg-gray-800 transition">
                Add to Cart
              </button>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
}
