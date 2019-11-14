using System;
using System.Text;
namespace Red_Back_Tree {
    public class Node<T> where T : IComparable {
        public Color color = Color.RED;
        public T data;
        public Node<T> parent = null;
        public Node<T> left = null;
        public Node<T> right = null;

        public Node (T data) {
            this.data = data;
        }

        public string ToString (bool isColor = false) {
            StringBuilder builder = new StringBuilder ();
            ToStringBuilder (builder, "", "", isColor);
            return builder.ToString ();
        }

        private void ToStringBuilder (StringBuilder builder, string prefix, String childrenPrefix, bool isColor = false) {

            builder.Append (prefix);
            if (isColor) {
                builder.Append (this.color);
            } else {
                builder.Append (this.data);
            }

            builder.Append ('\n');
            if (this.left != null && this.right != null) {
                this.left.ToStringBuilder (builder, childrenPrefix + "├── ", childrenPrefix + "│   ", isColor);
                this.right.ToStringBuilder (builder, childrenPrefix + "└── ", childrenPrefix + "    ", isColor);
            } else if (this.left != null) {
                this.left.ToStringBuilder (builder, childrenPrefix + "├── ", childrenPrefix + "│   ", isColor);
                builder.Append (childrenPrefix + "└── ");
                builder.Append ("NULL");
                builder.Append ('\n');
            } else if (this.right != null) {
                builder.Append (childrenPrefix + "├── ");
                builder.Append ("NULL");
                builder.Append ('\n');
                this.right.ToStringBuilder (builder, childrenPrefix + "└── ", childrenPrefix + "    ", isColor);
            }
        }
    }

}