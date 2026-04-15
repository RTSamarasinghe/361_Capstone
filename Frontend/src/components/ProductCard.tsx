import type { Product } from "../types/Product";

interface ProductCardProps {
  product: Product;
}

export default function Login({ product }: ProductCardProps) {
  return (
    <div key={product.id} className="">
      <img src={product.imageUrl} alt={product.name} className="" />

      <div className="">
        <h2 className="">{product.name}</h2>

        <p className="">${product.price.toFixed(2)}</p>

        <button className="">Add to Cart</button>
      </div>
    </div>
  );
}
