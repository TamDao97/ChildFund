package nts.swipesafe.adapter;

import android.content.Context;
import android.support.v7.widget.RecyclerView;
import android.util.TypedValue;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageView;
import android.widget.TextView;

import java.util.ArrayList;
import java.util.List;

import nts.swipesafe.R;
import nts.swipesafe.common.Utils;
import nts.swipesafe.model.ChildModel;

/**
 * {@link RecyclerView.Adapter}
 * TODO: Replace the implementation with code for your data type.
 */
public class ChildListAdapter extends RecyclerView.Adapter<ChildListAdapter.ViewHolder> {

    private final int mBackground;
    private List<ChildModel> listObject = new ArrayList<>();

    private final TypedValue mTypedValue = new TypedValue();

    private Context ctx;
    private OnItemClickListener mOnItemClickListener;

    public interface OnItemClickListener {
        void onEditClick(View view, int position, ChildModel obj);

        void onRemoveClick(View view, int position, ChildModel obj);
    }

    public void SetOnItemClickListener(final OnItemClickListener mItemClickListener) {
        this.mOnItemClickListener = mItemClickListener;
    }

    public class ViewHolder extends RecyclerView.ViewHolder {
        // each data item is just a string in this case
        public TextView txtName;
        public ImageView ivRemove, ivEdit;

        public ViewHolder(View v) {
            super(v);
            txtName = (TextView) v.findViewById(R.id.txtName);
            ivRemove = (ImageView) v.findViewById(R.id.ivRemove);
            ivEdit = (ImageView) v.findViewById(R.id.ivEdit);
        }
    }

    // Provide a suitable constructor (depends on the kind of dataset)
    public ChildListAdapter(Context ctx, List<ChildModel> items) {
        this.ctx = ctx;
        listObject = items != null ? items : new ArrayList<ChildModel>();
        ctx.getTheme().resolveAttribute(R.attr.selectableItemBackground, mTypedValue, true);
        mBackground = mTypedValue.resourceId;
    }

    @Override
    public ChildListAdapter.ViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        // create a new view
        View v = LayoutInflater.from(parent.getContext()).inflate(R.layout.item_child, parent, false);
        v.setBackgroundResource(mBackground);
        // set the view's size, margins, paddings and layout parameters
        ViewHolder vh = new ViewHolder(v);
        return vh;
    }

    // Replace the contents of a view (invoked by the layout manager)
    @Override
    public void onBindViewHolder(ViewHolder holder, final int position) {
        final ChildModel childModel = listObject.get(position);
        if (!Utils.isEmpty(childModel.Name)) {
            holder.txtName.setText(childModel.Name);
        } else {
            holder.txtName.setText("Không biết tên #" + String.valueOf(position + 1));
        }
        // view detail message conversation
        holder.ivRemove.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                if (mOnItemClickListener != null) {
                    mOnItemClickListener.onRemoveClick(view, position, childModel);
                }
            }
        });

        holder.ivEdit.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                if (mOnItemClickListener != null) {
                    mOnItemClickListener.onEditClick(view, position, childModel);
                }
            }
        });
    }

    // Return the size of your dataset (invoked by the layout manager)
    @Override
    public int getItemCount() {
        return listObject.size();
    }

    @Override
    public long getItemId(int position) {
        return position;
    }
}
