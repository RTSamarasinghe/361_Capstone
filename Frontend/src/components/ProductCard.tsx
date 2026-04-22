import type { Product } from "../types/Product";
import { useNavigate } from "react-router-dom";

interface ProductCardProps {
  product: Product;
}

export default function ProductCard({ product }: ProductCardProps) {
  const navigate = useNavigate();

  return (
    <div className="bg-white rounded-2xl shadow-md border border-gray-200 overflow-hidden hover:shadow-xl hover:-translate-y-1 transition duration-300">
      {/* Product Image */}
      <div className="aspect-square bg-gray-100 overflow-hidden">
        <img
          //src={`http://localhost:5208/Images/${product.imageUrl}`}
          src={product.imageUrl}
          alt={product.name}
          className="w-full h-full object-cover hover:scale-105 transition duration-300"
        />
      </div>

      {/* Product Info */}
      <div className="p-5">
        <h2 className="text-lg font-semibold text-gray-800 mb-2">
          {product.name}
        </h2>

        <p className="text-2xl font-bold text-blue-600 mb-4">
          ${product.price.toFixed(2)}
        </p>

        <button
          onClick={(e) => {
            navigate(`/products/${product.id}`);
          }}
          className="w-full py-2 rounded-md bg-blue-600 text-white font-medium hover:bg-blue-700 transition"
        >
          View More
        </button>
      </div>
    </div>
  );
}
