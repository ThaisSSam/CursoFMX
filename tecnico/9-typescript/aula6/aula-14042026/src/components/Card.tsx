import type { HTMLAttributes } from "react";

type CardProps = HTMLAttributes<HTMLDivElement> & {
  title: string;
  children: React.ReactNode;
  rounded: "sm" | "md" | "lg"
};

function Card({title, rounded, children, ...props}: CardProps){

  const borderVariant = () => {
    switch (rounded) {
      case "lg":
        return "rounded-lg"
      case "md":
        return "rounded-md"
      case "sm":
        return "rounded-sm"
    
      default:
        return "rounded-sm"
    }
  }

  return (
    <div className={`card ${borderVariant()}`} {...props}>
      <div className="card-header">
        <h2>{title}</h2>
      </div>
      <div className="card-content">
      {children}
      </div>
    </div>
  );
}

export default Card;