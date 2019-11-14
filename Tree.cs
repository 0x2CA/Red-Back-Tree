using System;
using System.Text;
namespace Red_Back_Tree {
    public class Tree<T> where T : IComparable {
        private Node<T> head = null;

        public Tree () { }

        public Tree (T data) {
            this.put (data);
        }

        public Tree (T[] datas) {
            for (int i = 0; i < datas.Length; i++) {
                this.put (datas[i]);
                Console.Write (this.ToString ());
                Console.Write (this.ToString (true));
            }
        }

        //添加数据
        public void put (T data) {
            Node<T> node = new Node<T> (data);

            //正常的二叉树插入
            Node<T> current = head;
            while (true) {
                if (current == null) {
                    // 为空的情况
                    this.head = node;
                    node.color = Color.BACK;
                    break;
                } else if (node.data.CompareTo (current.data) < 0) {
                    //比当前小
                    if (current.left != null) {
                        current = current.left;
                    } else {
                        current.left = node;
                        node.parent = current;
                        node.color = Color.RED;
                        break;
                    }
                } else if (node.data.CompareTo (current.data) >= 0) {
                    //比当前大
                    if (current.right != null) {
                        current = current.right;
                    } else {
                        current.right = node;
                        node.parent = current;
                        node.color = Color.RED;
                        break;
                    }
                }
            }

            //调整颜色
            this.putAdjustColor (node);
        }

        //添加修改颜色
        private void putAdjustColor (Node<T> node) {

            Node<T> father = node.parent;
            Node<T> grandFather = father?.parent;
            Node<T> uncle = father == grandFather?.left ? grandFather?.right : grandFather?.left;

            //父亲为黑色不需要调整
            if (father == null || father.color == Color.BACK) {
                return;
            } else if (father.color == Color.RED && (uncle != null && uncle.color == Color.RED)) {
                //父亲和兄弟都是红色，父亲和叔叔染黑，祖父染红
                father.color = Color.BACK;
                uncle.color = Color.BACK;
                grandFather.color = Color.RED;

                if (grandFather == head) {
                    head.color = Color.BACK;
                } else {
                    this.putAdjustColor (grandFather);
                }
            } else if (father.color == Color.RED && (uncle == null || uncle.color == Color.BACK)) {
                // 父亲为红色兄弟为黑色

                //父亲作为左儿子时候
                if (grandFather.left == father) {
                    //当前节点为左节点
                    if (node == father.left) {
                        //父亲染黑，祖父染红
                        father.color = Color.BACK;
                        grandFather.color = Color.RED;

                        this.rotateRight (grandFather);
                    } else {
                        this.rotateLeft (father);
                        this.putAdjustColor (father);
                    }
                } else {
                    //父亲作为右孩子时候

                    //当前节点为右节点
                    if (node == father.right) {
                        //父亲染黑，祖父染红
                        father.color = Color.BACK;
                        grandFather.color = Color.RED;

                        this.rotateLeft (grandFather);
                    } else {
                        this.rotateRight (father);
                        this.putAdjustColor (father);
                    }
                }

            }
        }

        public bool remove (T data) {
            Node<T> current = head;
            Node<T> node = null;

            // 搜索节点
            while (current != null) {
                if (current.data.CompareTo (data) < 0) {
                    //比当前小
                    current = current.left;
                } else if (current.data.CompareTo (data) > 0) {
                    //比当前大
                    current = current.right;
                } else {
                    node = current;
                    current = null;
                }
            }

            if (node != null) {
                return remove (node);
            }

            return false;
        }

        public bool remove (Node<T> node) {

            //孩子都为空
            if (node.left == null && node.right == null) {
                this.removeZeroChildrenNode (node);
            } else if (node.left != null && node.right != null) {
                //孩子都不为空
                this.removeTwoChildrenNode (node);
            } else {
                //其中一个为空
                this.removeOneChildrenNode (node);
            }

            return false;
        }

        //删除有两个孩子节点
        private void removeTwoChildrenNode (Node<T> node) {
            //寻找后继节点
            Node<T> descendantNode = this.getMinDescendantNode (node);
            //交换数据
            T dataTmp = descendantNode.data;
            descendantNode.data = node.data;
            node.data = descendantNode.data;

            //删除后继节点
            this.remove (descendantNode);
        }

        //获取最小的后继节点
        private Node<T> getMinDescendantNode (Node<T> node) {
            Node<T> right = node.right;
            if (right != null) {
                while (right.left != null) {
                    right = right.left;
                }
                return right;
            } else {
                return null;
            }
        }

        // 删除一个无孩子节点

        //TODO: 未完成
        private void removeZeroChildrenNode (Node<T> node) {
            Node<T> father = node.parent;
            Node<T> brother = father?.right == node?father?.left : father.right;

            if (node.color == Color.RED) {
                //删除节点为红色直接删除
                if (father.left == node) {
                    father.left = null;
                } else {
                    father.right = null;
                }
            } else {
                //删除节点为黑色

                if (father != null) {
                    //不为根节点

                    //兄弟节点没有孩子，兄弟节点一定是黑色
                    if (brother.left == null && brother.right == null) {
                        // 删减节点
                        if (father.left == node) {
                            father.left = null;
                        } else {
                            father.right = null;
                        }

                        // 兄弟染红，父亲染黑
                        brother.color = Color.RED;
                        father.color = Color.BACK;
                    } else if (!(brother.left != null && brother.right != null)) {
                        //兄弟结点有一个孩子不空
                        if ((brother == father.right && brother.right != null) || (brother == father.left && brother.left != null)) {
                            //该孩子和兄弟结点在同一边
                            // 删减节点
                            if (father.left == node) {
                                father.left = null;
                            } else {
                                father.right = null;
                            }

                            //父亲颜色给兄弟
                            brother.color = father.color;

                            //父亲染黑，兄弟子节点染黑,对父亲旋转
                            father.color = Color.BACK;
                            if (brother.right != null) {
                                brother.right.color = Color.BACK;
                                this.rotateLeft (father);
                            } else {
                                brother.right.color = Color.BACK;
                                this.rotateRight (father);
                            }

                        } else {
                            //该孩子和兄弟结点在不同一边
                            // 删减节点
                            if (father.left == node) {
                                father.left = null;
                            } else {
                                father.right = null;
                            }

                            //兄弟和父亲颜色互换,对兄弟旋转
                            Color tmpColor = father.color;
                            father.color = brother.color;
                            brother.color = tmpColor;

                            if (brother.right != null) {
                                this.rotateLeft (brother);
                            } else {
                                this.rotateRight (brother);
                            }

                        }
                    } else {
                        //兄弟右两个孩子

                        if (brother.color == Color.BACK) {

                        } else {

                        }
                    }
                    if (father.left == node) {
                        father.left = null;
                    } else {
                        father.right = null;
                    }
                } else {
                    // 根节点
                    this.head = null;
                }
            }
        }

        //删除有一个孩子节点
        private void removeOneChildrenNode (Node<T> node) {
            Node<T> father = node.parent;
            Node<T> brother = father?.right == node?father?.left : father.right;

            if (father != null) {
                if (node.left != null) {
                    if (father.left == node) {
                        father.left = node.left;
                    } else {
                        father.right = node.left;
                    }
                    node.left.parent = father;
                } else {
                    if (father.left == node) {
                        father.left = node.right;
                    } else {
                        father.right = node.right;
                    }
                    node.right.parent = father;
                }
            } else {
                //为根节点
                if (node.left != null) {
                    this.head = node.left;
                    node.left.parent = null;
                } else {
                    this.head = node.right;
                    node.right.parent = null;
                }
            }
        }
       
        //搜索
        public bool search (T data) {
            Node<T> current = head;
            while (current != null) {
                if (current.data.CompareTo (data) < 0) {
                    //比当前小
                    current = current.left;
                } else if (current.data.CompareTo (data) > 0) {
                    //比当前大
                    current = current.right;
                } else {
                    return true;
                }
            }
            return false;
        }

        //左旋
        private void rotateLeft (Node<T> node) {
            if (node != null && node.right != null) {
                Node<T> right = node.right;
                Node<T> father = node.parent;

                //右节点的左孩子作为当前节点右孩子
                node.right = right.left;
                if (node.right != null) {
                    node.right.parent = node;
                }

                //当前节点作为右节点的左孩子
                right.left = node;
                node.parent = right;

                //父亲节点
                right.parent = father;
                if (father != null) {
                    if (father.left == node) {
                        father.left = right;
                    } else {
                        father.right = right;
                    }
                } else {
                    head = right;
                    head.parent = null;
                }

            }
        }

        //右旋
        private void rotateRight (Node<T> node) {
            if (node != null && node.left != null) {
                Node<T> left = node.left;
                Node<T> father = node.parent;

                //左节点的右孩子作为当前节点左孩子
                node.left = left.right;
                if (node.left != null) {
                    node.left.parent = node;
                }

                //当前节点作为左节点的右孩子
                left.right = node;
                node.parent = left;

                //父节点   
                left.parent = father;
                if (father != null) {
                    if (father.left == node) {
                        father.left = left;
                    } else {
                        father.right = left;
                    }
                } else {
                    head = left;
                    head.parent = null;
                }
            }
        }

        // 树内容
        public string ToString (bool isColor = false) {
            if (this.head == null) {
                return "Empty";
            } else {
                return this.head.ToString (isColor);
            }
        }

        // 用于获得树的层数
        private int getDepth (Node<T> root) {
            return root == null ? 0 : (1 + Math.Max (getDepth (root.left), getDepth (root.right)));
        }

    }
}