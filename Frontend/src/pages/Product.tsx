import { useParams } from "react-router-dom";
import { useState } from "react";
import { mockProducts } from "../temp/mockProducts";
import type { Product as ProductType } from "../types/Product";

export default function Product() {
  const { id } = useParams();
  const [quantity, setQuantity] = useState(1);

  const product: ProductType | undefined = mockProducts.find(
    (p) => p.id === Number(id),
  );

  if (!product) {
    return <div className="text-gray-600">Product not found.</div>;
  }

  return (
    <div className="max-w-6xl mx-auto grid md:grid-cols-2 gap-10">
      {/* Image */}
      <div className="bg-white rounded-2xl shadow-md border border-gray-200 overflow-hidden">
        <img
          src={product.imageUrl}
          alt={product.name}
          className="w-full h-full object-cover"
        />
      </div>

      {/* Info */}
      <div className="flex flex-col">
        <h1 className="text-3xl font-bold text-gray-800 mb-3">
          {product.name}
        </h1>

        <p className="text-gray-600 mb-4">{product.description}</p>

        <p className="text-2xl font-bold text-blue-600 mb-6">
          ${product.price.toFixed(2)}
        </p>

        {/* Extra Info */}
        <div className="text-sm text-gray-500 mb-6 space-y-1">
          <p>Manufacturer: {product.manufacturuer}</p>
          <p>SKU: {product.sku}</p>
          <p>Stock: {product.stockQuantity}</p>
          <p>Rating: ⭐ {product.rating}</p>
        </div>

        {/* Quantity + Add to Cart */}
        <div className="flex items-center gap-4 mb-6">
          <input
            type="number"
            min={1}
            max={product.stockQuantity}
            value={quantity}
            onChange={(e) => setQuantity(Number(e.target.value))}
            className="w-20 px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
          />

          <button
            className="px-6 py-2 rounded-md bg-blue-600 text-white font-medium hover:bg-blue-700 transition"
            onClick={() =>
              console.log(`Add ${quantity} of ${product.name} to cart`)
            }
          >
            Add to Cart
          </button>
        </div>
      </div>
    </div>
  );
}
